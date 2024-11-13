using System;
using System.Numerics;

namespace Game10003
{
    public class Enemy
    {
        int health;
        int spriteSize = 50;
        Texture2D[] sprites;
        Vector2 position;
        int enemySpriteIndex = 0;
        float speed = 0.0005f;

        public Enemy(string enemyType, Vector2 startPosition)
        {
            if (enemyType == "spider")
            {
                health = 45;
                position = startPosition;
            }
        }

        public void Update()
        {

        }

        public void Render()
        {
            Draw.FillColor = Color.Red;
            Draw.Square(position, spriteSize);
        }

        public void Move(Vector2 newPosition)
        {
            position = position + (newPosition - position) * (speed * 10);
        }

    }
}
