# Systém pro evidenci rozpracovaných projektů

## Jak spustit projekt na raspberry?

1. Připojíme se na raspberry pomocí ssh na defaultního uživatele `pi` nebo vytvoříme nového s přístume k sudo 
2. Pomoci apt nainstalujeme potřebné package `sudo apt install curl postgresql git`
3. Na raspberry nainstalujeme .net core 6.0 a následně dotnet-ef pomocí příkazů
```
sh
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 6.0
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools' >> ~/.bashrc
source ~/.bashrc
dotnet tool install --global dotnet-ef
```
4. Naklonujeme projekt pomocí příkazu `git clone git@github.com:sps-trutnov/projekt-4ep-evidence.git --branch dev projekt-src` (Pro správné nastavení gitu můžete použít google :)
5. Přepneme se na uživatele `postgres` pomocí `sudo su postgres`
6. Vytvoříme nového uživatele pro postgres db pomocí `createuser jmeno_uzivatele -sP`, jmeno_uzivatele můžete zvolit jaké chcete, tento příkaz se vás zeptá na heslo pro tohoto nového uživatele, pro ukázku použiji heslo `password`
7. Vytvoříme novu db kterou bude vlastnit náš uživatel `createdb -O jmeno_uzivatele jmeno_db` - jmeno_db můžete zvolit jaké chcete
8. Z uživatele `postgres` se můžeme odhlásit
9. `cd ~/projekt-src/Source/EvidenceProject`
10. Upravíme `appsettings.json` aby vypadal následovně ![](https://i.kawaii.sh/rERgtMe.png)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Provider": "Postgres",
  "DatabaseConnection": "Host=localhost;Port:5432;Database=jmeno_db;Username=jmeno_uzivatele;Password=password"
}
```
11. Zkompilujeme projekt `dotnet build -c Release --use-current-runtime -o ~/projekt-dist -a arm64 --os linux` (artefakty se budou nacházet ve složce `~/projekt-dist`)
12. Nyní provedeme migraci databáze pomocí `cd ~/projekt-src/Source && dotnet-ef database update -p Migrations.Postgres --startup-project EvidenceProject`
13. Teď se můžeme dostat do složky `cd ~/projekt-dist` a projekt spustit pomocí `dotnet EvidenceProject.dll`
Hotovo!

