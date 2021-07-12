using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Atestat2._0.Models.Modele;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Atestat2._0.Models
{
    /// --> definite deja de framework. creează identitatea de bază pentru user
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    /// <--

    // definsesc baza de date
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new Initp());
        } // framework-ul mi-a inițializat deja baza de date
        
        // introduc in baza de date cele 4 tabele definite anterior ca și modele
        public DbSet<Carti> Carti { get; set; }
        public DbSet<Editura> Edituri { get; set; }
        public DbSet<Tip_coperta> Coperti { get; set; }
        public DbSet<Gen> Genuri { get; set; }
        public static ApplicationDbContext Create() // creez o nouă instanță a bazei de date (un fel de contructor de inițializare pentru baza de date)
        {
            return new ApplicationDbContext();
        }
    }

    public class Initp : DropCreateDatabaseAlways<ApplicationDbContext> // clasa de inițialzare a datelor din baza de date
    {
        protected override void Seed(ApplicationDbContext context) // Seed este funcția cu care introduc date în baza de date în forma ei inițială
                                                                   // ca să fie deja niște cărți pe site atunci când intru pe el
        {
            // "context" se comportă ca un obiect de tip bază de date (în el am date de tipul Carti, Editura, Gen, Tip_coperta, adică toate informațiile ce țin de o carte)
            
            // definesc câteva edituri
            Editura ed1 = new Editura { Editura_id = 100, Nume = "Humanitas" };
            Editura ed2 = new Editura { Editura_id = 102, Nume = "Polirom" };
            Editura ed3 = new Editura { Editura_id = 103, Nume = "Art" };
            Editura ed4 = new Editura { Editura_id = 104, Nume = "Paralela 45" };
            Editura ed5 = new Editura { Editura_id = 105, Nume = "Britanica" };
            Editura ed6 = new Editura { Editura_id = 106, Nume = "Arthur" };
            Editura ed7 = new Editura { Editura_id = 107, Nume = "Armada" };
            Editura ed8 = new Editura { Editura_id = 108, Nume = "Litera" };
            
            //adaug editurile în baza de date
            context.Edituri.Add(ed1);
            context.Edituri.Add(ed2);
            context.Edituri.Add(ed3);
            context.Edituri.Add(ed4);
            context.Edituri.Add(ed5);
            context.Edituri.Add(ed6);
            context.Edituri.Add(ed7);
            context.Edituri.Add(ed8);

            // definesc 2 coperți
            Tip_coperta tip1 = new Tip_coperta { Tip_carte_id = 300, Nume = "Hardcover" };
            Tip_coperta tip2 = new Tip_coperta { Tip_carte_id = 301, Nume = "Paperback" };
            
            // adaug coperțile în baza de date
            context.Coperti.Add(tip1);
            context.Coperti.Add(tip2);

            context.SaveChanges(); // salvez modificările facute la baza de date

            // încep să adaug cărți în baza de date
            context.Carti.Add(new Carti
            {
                Titlu = "Harry Potter și Camera Secretelor",
                Autor = "J.K. Rowling",
                Descriere = "Harry Potter are o vară plină: petrece o zi de naștere groaznică, primește avertizări sinistre de la un elf de casă pe nume Dobby și fuge de la familia Dursley cu mașina zburătoare a prietenului său Ron. La Hogwarts începe un nou an școlar, iar Harry aude niște șoapte ciudate pe coridoarele goale. Apoi au loc mai multe atacuri misterioase – previziunile sumbre ale lui Dobby par să se adeverească...",
                Nr_pagini = 400,
                Editura = ed6,
                Genuri = new List<Gen> // lista de genuri pentru cartea respectivă
                {
                    new Gen {Nume = "Fantasy"},
                    new Gen {Nume = "Aventură"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/1093427457-0.jpeg", // poză cu coperta cărții
                Categorie = "literatura",
                Pret = 48
            });

            context.Carti.Add(new Carti
            {
                Titlu = "Amurg: Zori de Zi",
                Autor = "Stephenie Meyer",
                Descriere = "A fi irevocabil indragostita de un vampir reprezinta pentru Bella Swan atat fantezie, cat si cosmar, impletite intr-o realitate extrem de periculoasa. Pe de-o parte este pasiunea ei intensa pentru vampirul Edward Cullen si pe de alta parte legatura stransa dintre ea si varcolacul Jacob Black, iar Bella a fost nevoita sa indure un an plin de tentatii tumultuoase, pierderi si lupte acerbe intre rivali pentru a ajunge la deznodamantul dorit. Decizia ei iminenta de a alege fie intunecata, dar seducatoarea lume a nemuritorilor, fie o viata simpla de muritoare reprezinta acum firul de care atarna soarta a doua triburi. Alegerea ei a declansat insa si o serie de evenimente neasteptate ce vor avea, probabil, consecinte inimaginabile. Iar viata Bellei ar putea fi distrusa pentru totdeauna. Mult asteptatul final al seriei 'Amurg', Zori de zi, ne dezvaluie in sfarsit secretele si misterele din aceasta poveste uimitoare care a incantat milioane de oameni.",
                Nr_pagini = 321,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Romance"}        
                },
                Tip_carte = tip2,
                image_link = "https://s13emagst.akamaized.net/products/959/958707/images/res_e38f41433cf0973a8c90e69a576338ef.jpg",
                Categorie = "literatura",
                Pret = 72
            });

            context.Carti.Add(new Carti
            {
                Titlu = "De veghe în lanul de secară",
                Autor = "J.D. Salinger",
                Descriere = "O nouă traducere pentru tinerii de astăzi. La cîteva decenii după traducerea de referință, Editura Polirom propune o versiune nouă, tînără, a romanului-cult De veghe în lanul de secară. Situată de ani de zile în topul preferințelor cititorilor din România, De veghe în lanul de secară continuă să fie, la peste cincizeci de ani de la apariție, cartea de căpătîi a adolescenței. Ce poate fi uimitor pentru tinerii care o citesc acum, la 16 ani, și ai căror părinți s-au născut, probabil, în aceeași perioada că și romanul, e că trec prin aceleași episoade, angoase și mirări precum Holden Caulfield. Doar că el nu folosea atît de des cuvîntul „cool” și nu naviga pe Internet. Iar cînd simțea nevoia s-o sune pe față de care era îndrăgostit, ori renunta, pentru că nu avea chef, ori întră în prima cabina de telefon. Problemele lui rămîn însă, și chiar și modul în care sînt rezolvate: prin revoltă, prin negare sau, uneori, prin ignorarea lor. Există voci care se întreabă de ce totuși a avut romanul un succes atît de uimitor, în toată lumea și, deja, de atîția ani. În fond, nu e singură carte care-și păstrează prospețimea cu trecerea timpului. E vorba despre altceva, un ceva despre care s-au scris zeci de mii de pagini. Ceva imposibil de sintetizat intr-o scurtă prezentare. Poate, pur și simplu, e o carte foarte-foarte bună. Atît. Alăturați-va milioanelor de tineri care au citit-o și veți înțelege.",
                Nr_pagini = 189,
                Editura = ed2,
                Genuri = new List<Gen>
                {
                    
                    new Gen {Nume = "Modern"}
                },
                Tip_carte = tip2,
                image_link = "http://mcdn.elefant.ro/mnresize/1500/1500/images/06/111306/de-veghe-in-lanul-de-secara_1_fullsize.jpg",
                Categorie = "literatura",
                Pret = 37
            });

            context.Carti.Add(new Carti
            {
                Titlu = "Ferma animalelor",
                Autor = "George Orwell",
                Descriere = "Ferma animalelor a inspirat în anul 1954 o celebră animaţie, iar în 1999 un film în care vocile personajelor au fost dublate, printre alţii, de Patrick Stewart, Julia Louis - Dreyfus şi Peter Ustinov. „Fiecare cuvînt de proză serioasă pe care l - am scris începînd din 1936 a fost îndreptat, direct sau indirect împotriva totalitarismului... Ferma animalelor este prima carte în care am încercat, în deplină cunoştinţă de cauză, să fac să fuzioneze scopul politic şi scopul artistic într - un tot unitar.” (George Orwell)",
                Nr_pagini = 260,
                Editura = ed2,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Alegorie"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/147494590-5.jpeg",
                Categorie = "literatura",
                Pret = 50
            });

            context.Carti.Add(new Carti
            {
                Titlu = "Poezii",
                Autor = "Mihai Eminescu",
                Descriere = "Placeholder description",
                Nr_pagini = 158,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Poezie"}
                },
                Tip_carte = tip2,
                image_link = "http://www.cartile-adevarul.com/images/adevarul-13-poezii.jpg",
                Categorie = "literatura",
                Pret = 30
            });

            context.Carti.Add(new Carti
            {
                Titlu = "Urzeala Tronurilor",
                Autor = "George R.R. Martin",
                Descriere = "Într-un ținut în care verile pot dura ani în șir și iernile, o viață, primejdia se arată la orizont. Vine iarna și, pe plaiurile înghețate de la nord de Winterfell, forțe întunecate și stranii se pregătesc de luptă în spatele Zidului ce protejează întregul regat Westeros. În mijlocul conflictului se află familia Stark de Winterfell, oameni la fel de aspri și neîndurători ca pământul pe care îl stăpânesc. Un tărâm al extremelor, de la fortărețele de piatră ale iernii, la castelele belșugului și ale verii, în care lorzi și prințese, soldați și vrăjitori, asasini și bastarzi iubesc, se luptă și uneltesc pentru a supraviețui.",
                Nr_pagini = 832,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Violență"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/932464152-0.jpeg",
                Categorie = "literatura",
                Pret = 89
            });

            context.Carti.Add(new Carti
            {
                Titlu = "Madame Bovary",
                Autor = "Gustave Flaubert",
                Descriere = "In a captivating tale of love and dissatisfaction, Gustave Flaubert continues to enthrall readers with his complex characters and powerful use of literary realism. The story follows Emma Bovary, a beautiful young woman who has grown tired of her dull and lifeless marriage. Soon after, Emma finds herself in a troubling situation of perpetual discontent when she abandons her husband for a series of desperate affairs. Widely recognized for its profoundly modern narrative, Madame Bovary is a time-honored favorite that resonates with readers long after the book is done. ",
                Nr_pagini = 400,
                Editura = ed6,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Dramă"}
                },
                Tip_carte = tip1,
                image_link = "https://cdn.dc5.ro/img-prod/875421940-0.jpeg",
                Categorie = "literatura",
                Pret = 55
            });

            context.Carti.Add(new Carti
            {
                Titlu = "Cei trei mușchetari",
                Autor = "Alexandre Dumas",
                Descriere = "Tânărul d’Artagnan sosește la Paris pentru a se alătura vestitului corp de gardă al regelui, dar constată aproape imediat că, din cauza firii sale impetuoase, ar trebui să se dueleze chiar cu unii dintre bărbații cărora a venit să le jure loialitate – Athos, Porthos și Aramis, trei prieteni inseparabili, cei trei muschetari. În scurt timp parte a grupului acestora, loialitatea lui d’Artagnan față de noii săi aliați îl face să intre în conflict cu mașinațiile cardinalului Richelieu și ale lui Milady, mâna sa dreaptă. Și atunci când tânărul erou se îndrăgostește de frumoasa, dar inaccesibila Constance, se trezește aruncat într-o lume a conspirațiilor și a minciunilor, iar singurii pe care se poate baza sunt prietenii săi, muschetarii. Poveste emoționantă despre prietenie și aventură.",
                Nr_pagini = 896,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Acțiune"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/1093427677-0.jpeg",
                Categorie = "literatura",
                Pret = 23
            });
            context.Carti.Add(new Carti
            {
                Titlu = "Solenoid",
                Autor = "Mircea Cărtărescu",
                Descriere = "„Primul meu gând, odată-ncheiată lectura, a fost să-mi scriu pe cartea de vizită «Eu am citit Solenoid». E un eveniment care-ți taie oarecum viața în două: cine va citi cartea va înceta să mai fie un cititor de rând.“ (Gabriel LIICEANU)",
                Nr_pagini = 864,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Modernism"}
                },
                Tip_carte = tip1,
                image_link = "https://cdn.dc5.ro/img-prod/1658288-0.jpeg",
                Categorie = "literatura",
                Pret = 47
            });
            context.Carti.Add(new Carti
            {
                Titlu = "Dune",
                Autor = "Frank Herbert",
                Descriere = "Intr-un viitor indepartat, intr-un imperiu interplanetar, condus dupa reguli feudale, casele nobiliare se lupta pentru suprematie. Casa Atreides primeste controlul asupra planetei Arrakis, singura sursa de mirodenie – cea mai valoroasa si importanta substanta din univers. Asta adanceste vechea rivalitate dintre Casele Atreides si Harkonnen. Leto Atreides si familia lui se vor confrunta nu doar cu o planeta desertica, neprietenoasa si necunoscuta, ci si cu comploturi politice periculoase. O poveste clasica, bogata in sensuri religioase, economice, tehnologice si sociale, o complexitate care o pune definitiv pe lista cartilor de citit intr-o viata.",
                Nr_pagini = 896,
                Editura = ed7,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Sci-Fi"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/274681822-0.jpeg",
                Categorie = "literatura",
                Pret = 65
            });
            context.Carti.Add(new Carti
            {
                Titlu = "Zbor deasupra unui cuib de cuci",
                Autor = "Ken Kesey",
                Descriere = "Zbor deasupra unui cuib de cuci, romanul care a lasat urme de nesters in literatura contemporana, este povestea zguduitoare a pacientilor dintr-un ospiciu condus cu mina de fier de sora-sefa Ratched, adepta unor metode de tratament barbare, precum electrosocurile si lobotomia. Singurul care i se opune este scandalagiul Randle Patrick McMurphy, simbolul revoltei impotriva autoritatii tiranice. Prin ochii altui pacient, seful Bromden, un indian urias, asistam la inclestarea dintre cei doi, care capata proportii aproape mitologice.",
                Nr_pagini = 408,
                Editura = ed2,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Comedie neagră"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/9789734633999-2484833.jpg",
                Categorie = "literatura",
                Pret = 25
            });
            context.Carti.Add(new Carti
            {
                Titlu = "Marele Gatsby",
                Autor = "F. Scott Fitzgerald",
                Descriere = "Capodopera lui F. Scott Fitzgerald, Marele Gatsby, publicată pe 10 aprilie 1925, a câștigat în timp în valoare: considerată inițial o carte spectaculară despre „generația pierdută“, astăzi ocupă unul dintre primele locuri pe lista celor mai bune romane ale secolului XX alcătuită de publicația New York Times. S-a bucurat de mai multe ecranizări și se estimează că s-a vândut în peste 25 de milioane de exemplare, cu traduceri în peste 40 de limbi.",
                Nr_pagini = 224,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Extravagant"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/1127376577-0.jpeg",
                Categorie = "literatura",
                Pret = 35

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Împăratul muștelor",
                Autor = "William Golding",
                Descriere = "Un grup de copii scapă cu viaţă din catastrofa prăbuşirii unui avion şi încearcă să supravieţuiască pe o insulă pustie din mijlocul Pacificului. Însă ceea ce ar putea fi aventura vieţii lor, o nouă robinsoniadă departe de adulţi şi de regulile acestora, se transformă într-un coşmar ucigător de vise: ura ia locul inocenţei şi crima se înstăpâneşte peste o lume care-şi rătăceste busola. O parabolă de o forţă incredibilă care ne oferă un răspuns deopotrivă crud şi realist la întrbarea: este libertatea absolută calea spre paradis?",
                Nr_pagini = 240,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Satiră"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/1976031-0.jpeg",
                Categorie = "literatura",
                Pret = 29
            });

            context.Carti.Add(new Carti
            {
                Titlu = "Codul lui Da Vinci",
                Autor = "Dan Brown",
                Descriere = "'Opera unui geniu.Dan Brown e unul dintre cei mai buni, cei mai inteligenţi scriitori.' - Nelson DeMille Aflat la Paris pentru o conferinţă, profesorul american de simbolistică şi istoria artei Robert Langdon primeşte un telefon în toiul nopţii. Bătrânul custode al Luvrului a fost asasinat în muzeu, în circumstanţe stranii, lăsând un mesaj la fel de enigmatic ca zâmbetul Giocondei. Împreună cu Sophie, specialistă în decriptare, Langdon începe o cursă frenetică pentru descifrarea numeroaselor indicii ascunse în picturile lui Leonardo... şi nu numai. Paşii îi poartă de la Paris la Londra şi apoi în Scoţia, pentru a dezvălui un mister păzit cu străşnicie de secole, capabil să zguduie din temelii dogmele creştinătăţii. Roman ecranizat în 2006 de Columbia Pictures.",
                Nr_pagini = 512,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Crime"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/9786066094931-2787084.jpg",
                Categorie = "literatura",
                Pret = 38
            });

            context.Carti.Add(new Carti
            {
                Titlu = "City of Bones",
                Autor = "Cassandra Clare",
                Descriere = "The tenth anniversary of Cassandra Clare’s phenomenal City of Bones demands a luxe new edition. The pride of any fan’s collection, City of Bones now has new cover art, over thirty interior illustrations, and six new full-page colour portraits of everyone’s favourite characters! This beautifully crafted collector’s item also includes a new piece written by Cassandra Clare. A perfect gift for the Shadowhunter fan in your life. This is the book where Clary Fray first discovered the Shadowhunters, a secret cadre of warriors dedicated to driving demons out of our world and back to their own. The book where she first met Jace Wayland, the best Shadowhunter of his generation. The book that started it all.",
                Nr_pagini = 528,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Mister"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/2326587-0.jpeg",
                Categorie = "literatura",
                Pret = 82
            });

            //Aici incep cartile nonliterare
            context.Carti.Add(new Carti
            {
                Titlu = "Istoria Artei",
                Autor = "E. H. Gombrich",
                Descriere = "În lumea întreagă, cititorii de toate vârstele şi de toate tipurile au văzut în prof. Gombrich un adevărat maestru care îmbină cunoaşterea şi înţelepciunea cu talentul unic de a-şi exprima deschis dragostea profundă pentru operele de artă pe care le descrie. Istoria artei îşi datorează popularitatea caracterului direct şi simplu al scrierii, precum şi priceperii autorului de a expune o naraţiune clară. Scopul său este acela de 'a aduce o oarecare ordine, o anumită claritate în mulţimea de nume proprii, date şi stiluri care complică puţin lucrările mai specializate'. Datorită cunoştinţelor despre psihologia artelor vizuale, Gombrich ne face să vedem istoria artei ca pe o 'înlănţuire neîntreruptă de tradiţii încă vii, care leagă arta epocii noastre de aceea din vremea piramidelor.'",
                Nr_pagini = 158,
                Editura = ed3,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Artă"}
                },
                Tip_carte = tip1,
                image_link = "https://cdn.dc5.ro/img-prod/1146176695-0.jpeg",
                Categorie = "non-literatura",
                Pret = 165

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Olandeza",
                Autor = "Robert Matzen",
                Descriere = "Audrey Hepburn a făcut parte din rezistență olandeză, a lucrat ca asistent medical în timpul bătăliei de la Arnhem, când a avut loc execuția brutală a unchiului ei și a trecut prin calvarul foametei din timpul iernii lui 1944.  Cartea prezintă amintirile ei, interviuri cu oameni care au cunoscut-o în război, jurnale de război și cercetări în arhivele olandeze secretizate.",
                Nr_pagini = 384,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Biografie"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/1061548177-0.jpeg",
                Categorie = "non-literatura",
                Pret = 49

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Familia Romanov",
                Autor = "Candace Fleming",
                Descriere = "Era cel mai bogat monarh din lume, avea 130 de milioane de supuși, iar imperiul său acoperea a șasea parte din uscat; însă Nicolae al II-lea al Rusiei n-a luat seama la mizeria extremă în care viețuia poporul său. Aflată mereu lângă țar, taciturna împărăteasă Alexandra trăia doar pentru familie și pentru obsesiile ei religioase, alimentate de viziunile unui mistic corupt, Rasputin. Înăuntrul palatelor, luxul nemăsurat al monarhului care conduce prin voie divină învăluie totul; dincolo de porțile lor, masele trăiesc împresurate de ororile Primului Război Mondial și de sărăcia care hrănește gânduri revoluționare. Curând, cele două lumi se vor ciocni, iar din această confruntare se va naște o nouă societate a terorii: Rusia Sovietică. Candace Fleming redă cu un incontestabil talent narativ destinul lui Nicolae al II-lea, ultimul autocrat rus, de la copilăria lui și a viitoarei sale soții până la abdicare și asasinarea întregii familii. ",
                Nr_pagini = 312,
                Editura = ed3,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Istorie"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/830387128-0.jpeg",
                Categorie = "non-literatura",
                Pret = 53

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Homo Deus",
                Autor = "Yuval Noah Harari",
                Descriere = "De-a lungul istoriei, omenirea s-a confruntat constant cu trei probleme cruciale: războiul, foametea şi molimele. În secolul XX, a reuşit să le rezolve în mare măsură. Războiul nu mai are aceeaşi putere de distrugere: în prezent numărul celor care se sinucid e mai mare decît al celor morţi în conflicte armate. Foametea dispare: oamenii suferă mai degrabă din cauza obezităţii decît din cauza malnutriţiei. Molimele nu mai fac ravagii: mai mulţi oameni mor de bătrîneţe decît în urma bolilor infecţioase. Ce anume va lua însă locul războiului, foametei şi molimelor în agenda omenirii? Ce destin vom alege pentru noi înşine, ce scopuri ne vom stabili? Yuval Noah Harari explorează proiectele, visurile şi coşmarurile care ne vor marca viitorul. Omul va încerca să devină asemenea zeilor, învingînd moartea şi creînd viaţa artificială. Este chiar următorul stadiu al evoluţiei – Homo deus.",
                Nr_pagini = 392,
                Editura = ed2,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Antropologie"}
                },
                Tip_carte = tip1,
                image_link = "https://cdn.dc5.ro/img-prod/27456228-0.jpeg",
                Categorie = "non-literatura",
                Pret = 70

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Dicționar de optimisme",
                Autor = "Chris Simion-Mercurian",
                Descriere = "Dictionarul de optimisme este cadoul perfect pentru acest sfarsit de an 2020... pentru ca:\n este primul dictionar / agenda in care fiecare dintre noi poate inventa cuvinte si fraze dragi sufletului sau;\n este un suflu de optimism, incredere si bunatate pentru oricine il citeste / foloseste;\n este sigurul dictionar / agenda care spune lucrurilor pe nume fara a se ascunde dupa motto - uri sau experiente de viata false.\n“Cand ti se pare ca nu mai poti, doar ti se pare.\nPoti mult mai mult.\nPastreaza - ti lacrimile pentru momentele de fericire.\nIn fiecare rasarit de soare, se naste ceva frumos.” – Chris Simion",
                Nr_pagini = 128,
                Editura = ed6,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Dicționar"}
                },
                Tip_carte = tip1,
                image_link = "https://cdn.dc5.ro/img-prod/945325314-0.jpeg",
                Categorie = "non-literatura",
                Pret = 39

            });

            context.Carti.Add(new Carti
            {
                Titlu = "12 Reguli de viață",
                Autor = "Jordan B. Peterson",
                Descriere = "Jordan B. Peterson este unul dintre cei mai sclipitori ganditori ai momentului. Psihologul canadian a devenit in ultimii ani un veritabil fenomen mediatic: pe YouTube poate fi vazut in peste 200 de prelegeri si interviuri, care aduna mai bine de 30 de milioane de vizualizari; iar pe Twitter si Facebook este urmarit de 300 000 de oameni. Plin de umor si asociind in mod surprinzator adevaruri ale traditiei occidentale cu ultimele descoperiri din neurostiinte, Peterson ne povesteste despre homari si asertivitate, despre skateboarding si adevaratii barbati, despre Iadul ranchiunii si arogantei, dar si despre lumina pe care o poate aduce in viata noastra mangaierea unei pisici sau un moment de sinceritate fata de sine – dar si fata de apropiatii nostri. Plecand de la revelatiile marilor religii ale lumii, dar si de la capodoperele literare si filosofice (ale unor Goethe, Nietzsche sau Dostoievski), autorul ne indeamna sa revenim la clasicele virtuti (disciplina, curaj, onestitate, prietenie etc.) si sa ne asumam raspunderea pentru existenta noastra tragica, in care exista totusi un sens ce astepta sa fie cautat. Intr-o lume in care structura familiei este in colaps, educatia este adesea impregnata de ideologie, iar societatea politica a ajuns sa fie periculos de polarizata, cele 12 Reguli de viata ofera un antidot: redescoperirea vechilor adevaruri si valori, care ne pot ajuta, in plina epoca relativista, sa ducem o viata mai buna, mai luminoasa, mai chibzuita.",
                Nr_pagini = 424,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Psihologic"}
                },
                Tip_carte = tip2,
                image_link = "http://mcdn.elefant.ro/mnresize/1500/1500/images/53/1329653/12-reguli-de-viata-un-antidot-la-haosul-din-jurul-nostru_1_fullsize.jpg",
                Categorie = "non-literatura",
                Pret = 59

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Preparate ușoare & și delicioase pentru toți",
                Autor = "Jamie Oliver",
                Descriere = "Jamie Oliver, unul dintre cei mai iubiți și citiți bucătari din lume, se întoarce cu idei pentru cele mai savuroase preparate vegetale. Cu toții încercăm să includem cât mai multe legume în alimentație, fie pentru a ne îmbunătăți sănătatea, fie pentru a reduce costul hranei sau a proteja planeta. Inspirat de diversitatea și ingeniozitatea bucătăriilor de pretutindeni, Jamie Oliver prezintă o gamă largă de mâncăruri ușor de preparat, create din dorința de a pune în valoare umila, dar uimitoarea LEGUMĂ. Dacă doriți să adoptați un stil de viață vegan, să luați o pauză de la carne două zile pe săptămână sau dacă, pur și simplu, doriți să încercați arome noi și surprinzătoare, această carte este pentru voi!",
                Nr_pagini = 158,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Culinar"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/899409708-0.jpeg",
                Categorie = "non-literatura",
                Pret = 120

            });

            context.Carti.Add(new Carti
            {
                Titlu = "365 de filme pe care trebuie să le vezi",
                Autor = "Geert Verbanck",
                Descriere = "Martin Scorsese, Orson Welles, Stanley Kubrick, Joel & Ethan Coen… se știe despre mulți mari regizori că au studiat obsesiv filmele clasice, ca să vadă cum sunt făcute. Și de fapt, putem și noi - muritorii de rând - să facem asta la fel de bine. Dar cum să-ncepi? Cum îți găsești calea prin jungla istoriei filmului? Ce anume poți învăța de la un film clasic? Cartea vă va da un răspuns la această întrebare și la multe altele. Nu doar că timp de un an vei vedea toate filmele clasice, dar vei avea și o perspectivă clară asupra lucrurilor pe care marile filme clasice le au în comun și asupra celor care fac o capodoperă să fie desăvârșită. Aceasta e busola și itinerarul tău, jurnalul care te va călăuzi prin magistrala lume a filmului.",
                Nr_pagini = 347,
                Editura = ed8,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Enciclopedie"},
                    new Gen {Nume = "Film"}
                },
                Tip_carte = tip1,
                image_link = "https://cdn.dc5.ro/img-prod/886265190-0.png",
                Categorie = "non-literatura",
                Pret = 50

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Vincent și Theo",
                Autor = "Deborah Heiligman",
                Descriere = "Vincent n-ar fi fost același fără Theo van Gogh. Călătorind prin volutele unei vieți chinuite, pictorul l-a avut mereu alături pe credinciosul său frate. Relația lor a fost zbuciumată, dar profundă, definitorie pentru amândoi. Minuțios documentată, bazată pe corespondența dintre Vincent și Theo, cartea lui Deborah Heiligman spune dramatica poveste a acestor destine și a devotamentului nemărginit care le-a legat.",
                Nr_pagini = 512,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Memorii"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/1063416485-0.jpeg",
                Categorie = "non-literatura",
                Pret = 37

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Gânduri către sine însuși",
                Autor = "Marcus Aurelius",
                Descriere = "Placeholder description",
                Nr_pagini = 188,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Etica și morală"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/773677900-1.jpeg",
                Categorie = "non-literatura",
                Pret = 30

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Teoria Universală",
                Autor = "Stephen W. Hawking",
                Descriere = "Placeholder description",
                Nr_pagini = 144,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Astronomie" }
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/99792153-0.jpeg",
                Categorie = "non-literatura",
                Pret = 26

            });

            context.Carti.Add(new Carti
            {
                Titlu = "One Direction: Who We Are - Our Official Autobiography",
                Autor = "One Direction",
                Descriere = "For the first time ever, global superstars One Direction are releasing their 100% official autobiography, offering a new, intimate insight into their lives as never before seen or heard! In their first in-depth autobiography, pop sensations Niall, Zayn, Liam, Harry and Louis tell the story of their lives. From nervously auditioning for the X Factor and meeting each other for the first time, to filming their hit movie This Is Us and releasing their bestselling third album, Midnight Memories, it really has been one incredible journey. For the first time, the boys’ loyal fans will be given an unprecedented insight into all of it, from their humble beginnings and lives before the X Factor, to recording their first single, touring the world, winning awards, breaking records, and much, much more.",
                Nr_pagini = 352,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Muzică"}
                },
                Tip_carte = tip1,
                image_link = "https://cdn.dc5.ro/img-prod/9780007577316-2728779.jpg",
                Categorie = "non-literatura",
                Pret = 64

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Jurnalul Annei Frank",
                Autor = "Anna Frank",
                Descriere = "Anne Frank este un nume familiar chiar și pentru cei care nu au citit Jurnalul. Îl poartă străzi, școli de pe toate meridianele. Îl poartă mult vizitatul muzeu din Amsterdam și fundația care l-a organizat în imobilul unde se afla Anexa secretă. Anne Frank și jurnalul ei figurează pe mai toate listele de excelență ale veacului XX privitoare la personalități și la cărți – Cei mai importanți oameni ai secolului, Cele mai bune cărți publicate în secolul XX, Operele literare definitorii pentru același secol, ca să numim doar câteva din multele topuri stabilite de specialiști, de ziariști ori chiar de marele public. Din 1947, când s-a publicat pentru prima oară în Țările de Jos, Jurnalul Annei Frank a fost tradus în peste 65 de limbi. S-a vândut în întreaga lume în peste 30 de milioane de exemplare. La celebritatea Jurnalului au contribuit neîndoielnic adaptările sale teatrale și cinematografice, faptul că a devenit materie de studiu în multe școli din lume, că a fost obiectul unor analize subtile, dar și ținta unor vehemente atacuri.Anne Frank a lăsat cea mai frapantă mărturie umană din timpul celui de-al Doilea Război Mondial. Mai mult, a dat un chip, o voce, un nume milioanelor de victime inocente – fără chip, fără voce, fără nume – ale barbariei naziste.",
                Nr_pagini = 392,
                Editura = ed1,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Jurnal"},
                    new Gen {Nume = "Biografie"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/1112261301-0.jpeg",
                Categorie = "non-literatura",
                Pret = 45

            });

            context.Carti.Add(new Carti
            {
                Titlu = "10 lecții pentru o lume post-pandemică",
                Autor = "Fareed Zakaria",
                Descriere = "Cartea jurnalistului american Fareed Zakaria e una dintre primele încercări de a creiona lumea care ne aşteaptă după ce va trece vîrful crizei provocate de pandemia de COVID-19. Analiza porneşte de la premisa că asistăm la unul dintre acele rare momente în care istoria îşi accelerează ritmul şi că această criză ce a afectat toate aspectele vieţii umane va reconfigura într-o măsură semnificativă lumea. Cele 10 „lecţii” care compun cartea acoperă toate domeniile, în primul rînd politica, dar şi tehnologia, economia ori viaţa noastră socială. Autorul argumentează că vom fi nevoiţi să ne reconsiderăm opiniile despre ceea ce înseamnă o guvernare eficientă, că, foarte probabil, domenii precum sănătatea şi educaţia vor ieşi de sub dominaţia absolută a pieţei, că globalizarea va căpăta un nou statut, mult mai ambiguu, şi că vor avea loc o creştere a autorităţii experţilor, dar şi o accentuare a inegalităţilor. 10 lecţii pentru o lume postpandemică vorbeşte despre trecut, prezent şi viitor şi va rămîne cu siguranţă una dintre analizele de căpătîi ale vieţii la începutul secolului XXI.",
                Nr_pagini = 250,
                Editura = ed2,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Sociologie"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/1013110217-0.jpeg",
                Categorie = "non-literatura",
                Pret = 35

            });

            context.Carti.Add(new Carti
            {
                Titlu = "Istoria frumuseții",
                Autor = "Umberto Eco",
                Descriere = "Cu toate că este ilustrată cu sute de capodopere ale tuturor timpurilor, aceasta nu este doar o istorie a artei. Imaginile, ca şi ampla antologie de texte de la Pitagora până în zilele noastre, ne ajută să reconstruim diferitele idei despre frumuseţe care s-au manifestat şi care au fost discutate începând cu Grecia antică şi ajungând până la noi. Cartea ilustrează modul în care a fost felurit concepută frumuseţea naturii, a florilor, a animalelor, a trupului omenesc, a astrelor, a raporturilor matematice, a luminii, a pietrelor preţioase, a veşmintelor, a lui Dumnezeu şi a Diavolului. Chiar dacă au ajuns până la noi doar textele filosofilor, ale scriitorilor, ale oamenilor de ştiinţă, ale misticilor sau ale teologilor, precum şi mărturiile artiştilor, cu ajutorul acestor documente poate fi reconstituit şi felul în care era percepută frumuseţea de către cei umili sau marginalizaţi, de către oamenii de rând din toate timpurile. Se poate vedea cum nu numai de-a lungul a mai multe epoci, ci uneori şi în cadrul aceleiaşi culturi, diferitele concepte de frumos au intrat în conflict unele cu altele. Le revine cititorilor, parcurgând aceste pagini, să hotărască dacă, prin nenumăratele sale întrupări, ideea de frumuseţe şi-a păstrat sau nu unele trăsături constante. Oricum, ei vor avea parte de o pasionantă aventură intelectuală şi emoţională.",
                Nr_pagini = 440,
                Editura = ed2,
                Genuri = new List<Gen>
                {
                    new Gen {Nume = "Pictură"}
                },
                Tip_carte = tip2,
                image_link = "https://cdn.dc5.ro/img-prod/489796904-1.jpeg",
                Categorie = "non-literatura",
                Pret = 129

            });


            context.SaveChanges(); // salvez modifcările făcute bazei de date
            base.Seed(context);
        }
    }
}