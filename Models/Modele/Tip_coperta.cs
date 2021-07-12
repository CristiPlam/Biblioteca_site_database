using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Atestat2._0.Models.Modele
{
    // Tabelul din baza de date pentru tipurile de coperți. O copertă are un id (număr de identificare; generat automat de baza de date) și un nume pentru tipul de copertă
    public class Tip_coperta
    {
        [Key] // cheia primară a tabelului
        public int Tip_carte_id { get; set; } // get și set sunt constructori de inițializare pentru fiecare variabilă din clasă

        [MinLength(5, ErrorMessage = "Numele tipului de carte nu poate fi mai scurt de 5 caractere"), // validări pentru numele unui tip de copertă
         MaxLength(30, ErrorMessage = "Numele tipului de carte nu poate fi mai lung de 30 de caractere")]
        public string Nume { get; set; }

        //relatie one-to-many cu tabelul Carti
        public virtual ICollection<Carti> Carti { get; set; } // creez colecția de cărți ce au un anumit tip de copertă (eg. 100 de cărți au copertă "hardback")
    }
}