using BepInEx;
using RoR2;
using System;

namespace StartingFunds
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Yawnsothoth.StartingFunds", "StartingFunds", "1.0.0")]
    public class StartingFunds : BaseUnityPlugin
    {
        bool hasRun = false;

        CharacterMaster currentPlayer = null;
        float currentDifficulty = -1;

        public void Awake()
        {
            On.RoR2.Run.Start += (orig, self) => {

                hasRun = false;
                currentPlayer = null;
                currentDifficulty = -1;

                orig(self);
            };

            On.RoR2.Run.BeginStage += (orig, self) =>
            {
                if (!hasRun)
                {
                    hasRun = true;
                    SetFunds(GetCurrentPlayer(), GetRunDifficulty());
                }

                orig(self);
            };
        }

        private CharacterMaster GetCurrentPlayer() 
        {
            if(currentPlayer == null)
            {
                currentPlayer = PlayerCharacterMasterController.instances[0].master;
            }

            return currentPlayer;
        }

        private float GetRunDifficulty()
        {
            if(currentDifficulty == -1)
            {
                currentDifficulty = RoR2.DifficultyCatalog.GetDifficultyDef(Run.instance.selectedDifficulty).scalingValue;
                if(currentDifficulty == 0f)
                {
                    currentDifficulty = 1f;    
                }
            }

            return currentDifficulty;
        }

        private void SetFunds(CharacterMaster currentPlayer, float difficulty)
        {
            uint startingWage = GetMoniesForDiffuculty(difficulty);
            currentPlayer.GiveMoney(startingWage);
            
        }

        private uint GetMoniesForDiffuculty(float difficulty)
        {
            uint baseMoney = (uint)(90 / (int)Math.Round(difficulty, 2));
            return baseMoney;
        }
    }
}
