using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Atestat2._0.Models.Modele
{
    // Tabelul din baza de date pentru Edituri. O editură are un id (număr de identificare; generat automat de baza de date) și un nume pentru editura respectivă.
    public class Editura
    {
        [Key] //cheia primară a tabelului
        public int Editura_id { get; set; } // get și set sunt constructori de inițializare pentru fiecare variabilă din clasă

        [MinLength(2, ErrorMessage = "Numele editurii nu poate fi mai scurt de 2 caractere"), // validări pentru numele editurii
         MaxLength(30, ErrorMessage = "Numele editurii nu poate fi mai lung de 30 de caractere")]
        public string Nume { get; set; }

        //relatie one-to-many cu tabelul Carti
        public virtual ICollection<Carti> Carti { get; set; } // creez colecția de cărți ce aparțin de o anumită editură
    }
}