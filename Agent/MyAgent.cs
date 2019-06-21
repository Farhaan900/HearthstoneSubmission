using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using SabberStoneCore.Tasks.PlayerTasks;
using System.IO;
using CsvHelper;
using System;
using System.Linq;
using SabberStoneCore.Enums;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Score;
using SabberStoneCore.Tasks.PlayerTasks;


namespace SabberStoneCoreAi.Agent
{
	class MyAgent : AbstractAgent
	{
		private Random Rnd = new Random();

		public override void FinalizeAgent()
		{
		}

		public override void FinalizeGame(int marker )
		{
		}

		public override PlayerTask GetMove(SabberStoneCoreAi.POGame.POGame poGame)
		{
			List<PlayerTask> simulatedactions = new List<PlayerTask>();
			simulatedactions.AddRange(poGame.CurrentPlayer.Options());
			Dictionary<PlayerTask, SabberStoneCoreAi.POGame.POGame> sim = poGame.Simulate(simulatedactions);

			Dictionary<PlayerTask, SabberStoneCoreAi.POGame.POGame>.KeyCollection keyColl = sim.Keys;
			Dictionary<int, PlayerTask> scoresKeyPair = new Dictionary<int, PlayerTask>();
			scoresKeyPair.Clear();

			try
			{

				int maxScore = Int32.MinValue;
				int _score = 0;

				foreach (PlayerTask key in keyColl)
				{
					//Console.WriteLine(key);
					//Console.WriteLine("Player num -->>>>"+poGame.CurrentPlayer.PlayerId);
					//Console.WriteLine("SIM  -->>>>"+sim[key]);


					if (sim[key] == null)
						continue;

					if (key.PlayerTaskType == PlayerTaskType.END_TURN)
					{
						_score = Int32.MinValue + 1;
					}

					else
						_score = Score(sim[key], poGame.CurrentPlayer.PlayerId);
					//Console.WriteLine(_score);
					if (!scoresKeyPair.ContainsKey(_score))
						scoresKeyPair.Add(_score, key);
					if (_score > maxScore)
						maxScore = _score;

				}

				//Console.WriteLine("Played ==>> "+scoresKeyPair[maxScore]);
				return scoresKeyPair[maxScore];
			}
			catch
			{
				return poGame.CurrentPlayer.Options()[Rnd.Next(poGame.CurrentPlayer.Options().Count)];
			}
		}

		private static int Score(POGame.POGame state, int playerId)
		{
			var p = state.CurrentPlayer.PlayerId == playerId ? state.CurrentPlayer : state.CurrentOpponent;
			switch (state.CurrentPlayer.HeroClass)
			{
				case CardClass.MAGE: return new MohammedsMageMontage { Controller = p }.Rate();
				case CardClass.WARRIOR: return new MohammedsWarriorrsWhip { Controller = p }.Rate();
				case CardClass.SHAMAN: return new MohammedsShamanKiller { Controller = p }.Rate();
				case CardClass.HUNTER: return new MohammedsHunterHonor { Controller = p }.Rate();
				case CardClass.ROGUE: return new MohammedsGlobalDominator { Controller = p }.Rate();
				default: return new MohammedsGlobalDominator { Controller = p }.Rate();
			}
		}

		public override void InitializeAgent()
		{
			Rnd = new Random();
		}

		public override void InitializeGame()
		{
		}
	}
}
