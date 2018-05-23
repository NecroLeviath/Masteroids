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
        static List<int> mastHighscore = new List<int>();
        static List<int> astHighscore = new List<int>();
        SpriteFont buttonFont;

        public HighScoreState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content,EntityManager entityManager,State previousState) : base(game, graphicsDevice, content)
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

            var pos = new Vector2(650,500);
            spriteBatch.DrawString(buttonFont, "MASTEROIDS",pos, Color.White);
            for (int i = mastHighscore.Count - 1; i >= 0; i--)
            {
                pos.X = 750;
                pos.Y += 30;
                var text = mastHighscore[i].ToString();
                var x = buttonFont.MeasureString(text).X/2;
                var offset = new Vector2(x,0);
                spriteBatch.DrawString(buttonFont, text, pos-offset, Color.White);
            }
            pos = new Vector2(1100,500);
            spriteBatch.DrawString(buttonFont, "ASTEROIDS", pos, Color.White);
            for (int i = astHighscore.Count - 1; i >= 0; i--)
            {
                pos.X = 1190;
                pos.Y += 30;
                var text = astHighscore[i].ToString();
                var x = buttonFont.MeasureString(text).X / 2;
                var offset = new Vector2(x, 0);
                spriteBatch.DrawString(buttonFont, text, pos-offset, Color.White);
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

        public static void GetHighscore()
        {
            var mastS = File.ReadAllLines(@".../.../.../.../Content/mastHighscore.txt").ToList();
            mastHighscore = mastS.Select(x =>
            {
                int r;
                if (int.TryParse(x.ToString(), out r))
                    return r;
                else
                    return 0;
            }).ToList();

            var astS = File.ReadAllLines(@".../.../.../.../Content/astHighscore.txt").ToList();
            astHighscore = astS.Select(x =>
            {
                int r;
                if (int.TryParse(x.ToString(), out r))
                    return r;
                else
                    return 0;
            }).ToList();
        }
        
        public static void SetMasteroidScore(int score)
        {
            mastHighscore.Add(score);
            mastHighscore.Sort();
            while (mastHighscore.Count > 10)
                mastHighscore.RemoveAt(0);
            var mastS = mastHighscore.Select(x => x.ToString()).ToArray();
            File.WriteAllLines(".../.../.../.../Content/mastHighscore.txt", mastS);
        }
        
        public static void SetAsteoidScore(int score)
        {
            astHighscore.Add(score);
            astHighscore.Sort();
            while (astHighscore.Count > 10)
                astHighscore.RemoveAt(0);
            var astS = astHighscore.Select(x => x.ToString()).ToArray();
            File.WriteAllLines(".../.../.../.../Content/astHighscore.txt", astS);

        }



    }
}
