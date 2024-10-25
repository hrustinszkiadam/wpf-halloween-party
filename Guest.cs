namespace halloween_party
{
    internal class Guest
    {
        public string Name { get; set; }
        public int Age { get; set; }
		public string Costume { get; set; }

        public static List<Guest> Guests = new List<Guest>();

        public static List<string> Costumes = new List<string>()
		{
			"Vámpír",
			"Boszorkány",
			"Csontváz",
			"Zombi",
			"Ördög",
			"Kísértet",
			"Pókember",
			"Batman",
			"Rendőr",
			"Csodanő",
			"Kalóz",
			"Orvos",
			"Király"
		};

		public Guest(string name, int age, string costume)
		{
			Name = name;
			Age = age;
			Costume = costume;
		}

		public override string ToString()
		{
			return $"{Name} ({Age}) - {Costume}";
		}

		public static void LoadGuests(string path)
		{
			string[] lines = System.IO.File.ReadAllLines(path);
			foreach (string line in lines)
			{
				string[] parts = line.Split(';');
				if (parts[1] == "") throw new Exception("Nem érvényes file");
				int.TryParse(parts[1], out int age);
				if (parts.Length != 3 || age == 0) throw new Exception("Nem érvényes file");
			}

			foreach (string line in lines)
			{
				string[] parts = line.Split(';');
				Guests.Add(new Guest(parts[0], int.Parse(parts[1]), parts[2]));
			}
		}

		public static void SaveGuests(string path)
		{
			List<string> lines = new List<string>();
			foreach (Guest guest in Guests)
			{
				lines.Add($"{guest.Name};{guest.Age};{guest.Costume}");
			}
			System.IO.File.WriteAllLines(path, lines);
		}
	}
}
