using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnboardingCS.Models
{
    public class TodoItem
    {
        //EF creates a null column for all reference type properties and nullable primitive properties e.g. string, Nullable<int>, Student, Grade (all class type properties)
        //EF creates NotNull columns for Primary Key properties and non-nullable value type properties e.g. int, float, decimal, datetime etc.
        //Cascade delete	Enabled by default for all types of relationships.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TodoId { get; set; } 
        [Required]
        public string TodoName { get; set; }
        [Required]
        public bool TodoIsDone { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? LabelId { get; set; } //Convention 4 to get access directlyto FK
        [JsonIgnore]
        [ForeignKey("LabelId")] 
        //optional, if not implement will follow code conventions for PK or
        //By default EF will look for the foreign key property with the same name as the principal entity primary key 
        //If the foreign key property does not exist, then EF will create an FK column in the Db table with<Dependent Navigation Property Name> + "_" + <Principal Entity Primary Key Property Name
        //e.g.EF will create Grade_GradeId foreign key column in the Students table if the Student entity does not contain foreignkey property for Grade.
        public Label Label { get; set; }
    }
}
