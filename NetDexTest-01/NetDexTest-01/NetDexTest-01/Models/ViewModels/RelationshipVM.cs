using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Services;
using NetDexTest_01.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace NetDexTest_01.Models.ViewModels
{

    public class RelationshipRequest
    {
        public int? Id { get; set; }
        public string input { get; set; }
        public string nicknameOne { get; set; }
        public string nicknameTwo { get; set; }
        public string? description { get; set; }
    }


    public class RelationshipRequestExtend : RelationshipRequest
    {
        public int? ParentId { get; set; } = 0;
        public int? ChildId { get; set; } = 0;

    }





    public class RelationshipVM
    {
        public RelationshipVM()
        {

        }

        public string? AppEmail { get; set; }
        public int? Id { get; set; } = 0;
        public string AppUsername { get; set; }
        public int? PersonParentId { get; set; }
        public string? ParentNickname { get; set; }
        public string RelationshipDescription { get; set; } = string.Empty;
        public int? PersonChildId { get; set; }
        public string? ChildNickname { get; set; }
        public DateTime? LastUpdated { get; set; }


    }
}