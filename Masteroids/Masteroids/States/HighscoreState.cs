using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Masteroids.Controls;
using System.IO;

namespace Masteroids
{
    class HighScoreState : State
    {
        List<Component> components;
        EntityManager entityMgr;
        State previousState;
        SpriteFont buttonFont;

        public HighScoreState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, EntityManager entityManager, State previousState) : base(game, graphicsDevice, content)
        {
            entityMgr = entityManager;
            Texture2D buttonTexture = content.Load<Texture2D>("button");
            buttonFont = content.Load<SpriteFont>("Fonts/MenuFont");
            int x = graphicsDevice.Viewport.Width;
            int y = graphicsDevice.Viewport.Height;
            this.previousState = previousState;
            Button ReturnButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2((x - buttonTexture.Width) / 2, 950),
                Text = "Return"
            };
            ReturnButton.Click += ReturnButton_click;
            components = new List<Component>()
            {
                ReturnButton,
            };
        }

        private void ReturnButton_click(object sender, EventArgs e)
        {
            game.ChangeState(previousState);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            entityMgr.Draw(spriteBatch);
            foreach (Masteroids.Component component in components)
                component.Draw(gameTime, spriteBatch);

            var pos = new Vector2(650, 500);
            spriteBatch.DrawString(buttonFont, "MASTEROIDS", pos, Color.White);
            for (int i = 0; i < mastStringScore.Count; i++)
            {
                pos.Y += 30;
                var text = mastStringScore[i];
                spriteBatch.DrawString(buttonFont, text, pos, Color.White);
            }
            pos = new Vector2(1100, 500);
            spriteBatch.DrawString(buttonFont, "ASTEROIDS", pos, Color.White);
            for (int i = 0; i < astStringScore.Count; i++)
            {
                pos.Y += 30;
                var text = astStringScore[i];
                spriteBatch.DrawString(buttonFont, text, pos, Color.White);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (Masteroids.Component component in components)
                component.Update(gameTime);
            entityMgr.Update(gameTime);
        }

        #region Highscore manipulation
        static List<Tuple<string, int>> masteroidsHighscore = new List<Tuple<string, int>>();
        static List<string> mastStringScore = new List<string>();
        static List<Tuple<string, int>> asteroidsHighscore = new List<Tuple<string, int>>();
        static List<string> astStringScore = new List<string>();

        public static void GetHighscore()
        {
            RetrieveScore(@".../.../.../.../Content/mastHighscore.txt", masteroidsHighscore, ref mastStringScore);
            RetrieveScore(@".../.../.../.../Content/astHighscore.txt", asteroidsHighscore, ref astStringScore);
        }

        private static void RetrieveScore(string path, List<Tuple<string, int>> tupleList, ref List<string> stringList)
        {
            stringList = File.ReadAllLines(path).ToList();                  // Reads from external text file
            var splitList = stringList.Select(x => x.Split(' ')).ToList();  // Splits the strings into "name" and "score" strings
            var names = splitList.Select(x => x[0]).ToList();               // Adds all names to a list
            var scores = splitList.Select(x =>                              // Adds all scores to a list
            {
                if (int.TryParse(x[1], out int r))  // Converts the number strings into integers
                    return r;
                else
                    return 0;                       // If it can't convert it, it returns 0 instead
            }).ToList();
            for (int i = 0; i < splitList.Count; i++)                       // Adds all names and scores to a list
            {
                var tuple = Tuple.Create(names[i], scores[i]);  // Creates a tuple of the name and score
                tupleList.Add(tuple);                           // Adds the tuple to a list
            }
        }

        public static void SetMasteroidScore(string name, int score)
        {
            SetScore(@".../.../.../.../Content/mastHighscore.txt", name, score, masteroidsHighscore, ref mastStringScore);
        }

        public static void SetAsteroidScore(string name, int score)
        {
            SetScore(@".../.../.../.../Content/astHighscore.txt", name, score, asteroidsHighscore, ref astStringScore);
        }

        private static void SetScore(string path, string name, int score, List<Tuple<string, int>> tupleList, ref List<string> stringList)
        {
            tupleList.Add(Tuple.Create(name, score));                   // Creates a tuple with the name and the score and adds it to the tuple list
            tupleList.Sort((x, y) => y.Item2.CompareTo(x.Item2));       // Sorts the list by score
            while (tupleList.Count > 10)                                // Checks if there are more than 10 saved scores...
                tupleList.RemoveAt(10);                                 // ... and removes the last score if there are
            stringList = tupleList.Select(x =>                          // Updates the string list to match the tuple list
            {
                var result = x.Item1 + " ";
                for (int i = 0; i < 9 - x.Item2.ToString().Length; i++)
                    result += '0';
                result += x.Item2;
                return result;
            }).ToList();
            File.WriteAllLines(path, stringList.ToArray());             // Writes to external text file
        }
        #endregion
    }
}
