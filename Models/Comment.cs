using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
    


namespace Wall_Assign{

    public class Comment {
        [Key]

        public int CommentId {get;set;}

        public string ConContent{get;set;}

        public int UserId {get;set;}

        public int MessageId {get;set;}

        public User Navuser {get;set;}

        public Message Navmessage {get;set;}
    }



}