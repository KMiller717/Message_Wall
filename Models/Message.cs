using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
    


namespace Wall_Assign{

    public class Message{

        [Key]

        public int MessageId {get;set;}

        [Required]
        public string MesContent {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public int UserId {get;set;}

        public User Creator {get;set;}

        public List<Comment> Comments {get;set;}






    }




}