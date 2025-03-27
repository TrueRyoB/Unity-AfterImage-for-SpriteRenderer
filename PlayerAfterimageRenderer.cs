using System.Collections.Generic;
using UnityEngine;
using Fujin.Mobs;

namespace Fujin.Player
{
    public class AfterimageRenderer : MonoBehaviour
    {
        [SerializeField] Material material;
        [SerializeField] int duration = 150;

        private SpriteRenderer _spriteRenderer;
        private readonly Stack<AfterImage> _pool = new Stack<AfterImage>();
        private readonly Queue<AfterImage> _renderQueue = new Queue<AfterImage>();

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.Log("It's null!");
            }
        }

        private Vector3 _oldPosition;
        private int _count;
        private readonly int _intervalFrames = 5;

        void Update()
        {
            Render();
            
            //以下テスト用
            if (transform.position == _oldPosition)
            {
                _count = 0;
                return;
            }
            if (_count >= _intervalFrames)
            {
                _count = 0;
                Enqueue();
            }
            _oldPosition = transform.position;
            _count++;
        }

        public void Render()
        {
            for (int i = 0; i < _renderQueue.Count; i++)
            {
                var afterimage = _renderQueue.Dequeue();
                afterimage.RenderSprites();

                if (afterimage.FrameCount < duration)
                {
                    _renderQueue.Enqueue(afterimage);
                }
                else
                {
                    afterimage.Reset();
                    _pool.Push(afterimage);
                }
            }
        }
        
        public void Enqueue()
        {
            AfterImage afterimage = (_pool.Count > 0) ? _pool.Pop() : new AfterImage();
            afterimage.Setup(_spriteRenderer);
            _renderQueue.Enqueue(afterimage);
            Debug.Log("Enqueued another afterimage!");
        } 
    }
}