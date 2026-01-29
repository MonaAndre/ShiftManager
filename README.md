## Krav för G (Godkänt)

- [x] Minst 3 entiteter
- [x] Minst en 1–M-relation
- [x] Minst en M–M-relation
- [x] Code first med minst 1 migration
- [x] Minst 3 constraints (t.ex. [Required] , [MaxLength] , unique index), gäller ej foreign key constraints
- [ ] Seed-data för att kunna testa applikationen
- [ ] Kunna skapa, läsa, uppdatera och ta bort data (CRUD)
- [ ] Minst 1 operation som hanterar relationer (t.ex. lägga till/ta bort koppling i M–M)
- [ ] Använda Include för att hämta relaterad data
- [ ] README som innehåller instruktioner för seed och att köra projektet


## Krav för VG (Väl Godkänt)Krav för VG (Väl Godkänt)

Utöver samtliga G-krav ska du uppfylla alla följande krav:
- [x] Minst 4 entiteter
- [ ] Minst 3 migrationer
- [x] Minst 5 constraints konfigurerade med Fluent API i OnModelCreating (t.ex. IsRequired() , HasMaxLength() , HasIndex() , HasDefaultValue() ), gäller
       ej foreign key constraints
- [ ] Använd AsNoTracking() på samtliga läsoperationer där data inte ska uppdateras
- [ ] Använd projektering med Select() för att endast hämta nödvändig data (minst 1 query)
- [ ] Minst 1 aggregering (t.ex. Count() , Sum() , Average() , GroupBy )
- [ ] Använd explicit transaktion ( BeginTransaction / CommitAsync ) för minst 1 operation som involverar flera relaterade skrivningar


## InlämningInlämning

ZIP-fil med projektet inklusive en README-fil.
