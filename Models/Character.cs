using System.Collections.Generic;

namespace dotnet_rpg.Models
{
    public class Character
    {
        public int CharacterId { get; set; }
        public string Name { get; set; } = "Bobert";
        public int HitPoints { get; set; } = 80;
        public int Mana { get; set; } = 5;
        public int Strength { get; set; } = 12;
        public int Intelligence { get; set; } = 5;
        public int Agility { get; set; } = 7;
        public RpgClass Class { get; set; } = RpgClass.Knight;
        public User User { get; set; }
        public Weapon Weapon { get; set; }
        public List<Skill> Skill { get; set; }
    }
}