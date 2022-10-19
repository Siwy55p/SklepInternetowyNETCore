# partner-aluro

projekt VS 2022 partner-aluro.sln
C:\User\Siwy\source\repos\partner-aluro - SiwyPC
C:\Users\Robert\Source\Repos\Siwy55p\partner-aluro - Robert PC

NuGet
Potrzebne do korzystania Entity Framework, aby polaczyc sie z nasza baz¹ danych
1.EntutyFramework 6.0.9
2.EntityFrameworkCore.Tools  6.0.9 - do przeprowadzania migracji
3.EntityFrameworkCore.SqlServer 6.0.9 - konfiguracje do testow serwera
Dobrze jest miec SQL Server Management Studio i lokaln¹ bazê DbTest 
SQL Server 2012 Express + Management Studio

Pierwsza migracja - z poziomu Konsoli manadzera pakietów
Add-migration init
Update-database

CRUD

4.Microsoft.AspNetCore.Identity 2.2.0
5.AspNetCore Identity.EntityFrameworkCore 6.0.9

add-migration AddUsers
update-database

Przy uruchamianiu repozytorium na nowym stanowisku pracy nalezy wykonaæ 
add-migration init
update-database - z poziomu Konsoli manadzera pakietów. Only one. Baza tworzona na lokalnym stanowisku pracy - bez danych.

19.09.2022 utworzenie hostingu webio.pl
migracja bazy - pomyslne zbudowanie bazy

add-migration Category
update-database

#27.09.2022 
usuniecie plików migracyjnych w komputerze roboczym Robert i utworzenie bazy i nowej migracji na nowo
remove-migration 
add-migration
update-database Init
(localdb)\\mssqllocaldb;Database=DbTest;
