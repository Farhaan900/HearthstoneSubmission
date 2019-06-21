using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCoreAi.Score
{
	public class MohammedsHunterHonor : Score
	{
		public override int Rate()
		{
			if (OpHeroHp < 1)
				return Int32.MaxValue;

			if (HeroHp < 1)
				return Int32.MinValue;

			int result = 0;

			if (OpBoardZone.Count == 0 && BoardZone.Count > 0)
				//result += 1000;
				result += 400;
			//else
			result += (BoardZone.Count - OpBoardZone.Count) * 50;

			//if (OpMinionTotHealthTaunt > 0)
			//	result += OpMinionTotHealthTaunt * -1000;

			result += (MinionTotHealthTaunt - OpMinionTotHealthTaunt) * 600;

			result += (MinionTotHealth - OpMinionTotHealth) * 250;

			result += (MinionTotAtk - OpMinionTotAtk) * 450;

			result += (HeroHp - OpHeroHp) * 310;

			result += HeroAtk * 910;

			result += (HandCnt - OpHandCnt) * 20;
			/**
			if (DeckCnt == 0)
				result -= 700;

			if (OpDeckCnt == 0)
				result += 900;
	**/
			return result;
		}

		public override Func<List<IPlayable>, List<int>> MulliganRule()
		{
			return p => p.Where(t => t.Cost > 3).Select(t => t.Id).ToList();
		}
	}
}
