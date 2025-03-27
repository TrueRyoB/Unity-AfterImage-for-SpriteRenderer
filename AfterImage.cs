using UnityEngine;
using System;

namespace Fujin.Mobs
{
    [Serializable]
    public class AfterImage
    {
        private Material _material; 
        private Sprite _sprite;
        Mesh _mesh;
        Matrix4x4 _matrix;
        public float FrameCount { get; private set; }
        private static int _lifeSpan;

        public AfterImage()
        {
            Reset();
        }

        public static void SetLifeSpan(int span)
        {
            _lifeSpan = span;
        }

        public static void SetGradient(Gradient g)
        {
            _g = g;
        }

        public void Reset()
        {
            FrameCount = 0;
        }
        
        public void Setup(SpriteRenderer spriteRenderer, Material material = null)
        {
            _material = material ? material : spriteRenderer.material;
            _sprite = spriteRenderer.sprite;
            
            _matrix = spriteRenderer.transform.localToWorldMatrix;

            _mesh = CreateSpriteMesh(_sprite);

            _propertyBlock ??= new MaterialPropertyBlock();
            _propertyBlock.SetColor(ColorIndex, _g?.Evaluate(0) ?? Color.white);
            
            Graphics.DrawMesh(
                _mesh,
                _matrix,
                _material,
                0 ,
                null,
                0,
                _propertyBlock
            );
        }

        private MaterialPropertyBlock _propertyBlock;
        private static readonly int ColorIndex = Shader.PropertyToID("_Color");
        private static Gradient _g;
        
        public void RenderSprites()
        {
            _propertyBlock ??= new MaterialPropertyBlock();
            if (_lifeSpan != 0)
            {
                _propertyBlock.SetColor(ColorIndex, _g?.Evaluate(FrameCount / _lifeSpan) ?? Color.white);
            }
            
            Graphics.DrawMesh(
                _mesh,
                _matrix,
                _material,
                0 ,
                null,
                0,
                _propertyBlock
            );

            FrameCount += Time.timeScale;
        }
        
        private Mesh CreateSpriteMesh(Sprite sprite)
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = Array.ConvertAll(sprite.vertices, v => (Vector3)v);
            mesh.vertices = vertices;

            mesh.uv = sprite.uv;

            mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);

            return mesh;
        }
    }
}

