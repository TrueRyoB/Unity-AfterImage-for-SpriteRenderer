using UnityEngine;
using System;
using UnityEngine;

namespace Fujin.Mobs
{
    [Serializable]
    public class AfterImage
    {
        private Material _material; 
        private Sprite _sprite;
        private SpriteRenderer _renderer;
        public int FrameCount { get; private set; }

        public AfterImage()
        {
            Reset();
        }

        public void Reset()
        {
            FrameCount = 0;
        }
        
        public void Setup(SpriteRenderer spriteRenderer, Material material = null)
        {
            // SpriteRendererを保存（位置情報はRenderSprites()で更新する）
            _renderer = spriteRenderer;
            _material = material != null ? material : spriteRenderer.material;
            _sprite = spriteRenderer.sprite;
        }
        
        public void RenderSprites()
        {
            if (_sprite == null || _material == null || _renderer == null) return;

            Matrix4x4 matrix = _renderer.transform.localToWorldMatrix;

            Mesh spriteMesh = CreateSpriteMesh(_sprite);

            Graphics.DrawMesh(
                spriteMesh,
                matrix,
                _material,
                0 
            );
            FrameCount++;
        }

        /// <summary>
        /// スプライトからMeshを生成する
        /// </summary>
        private Mesh CreateSpriteMesh(Sprite sprite)
        {
            Mesh mesh = new Mesh();
            
            // 頂点情報（スプライトの形）
            Vector3[] vertices = Array.ConvertAll(sprite.vertices, v => (Vector3)v);
            mesh.vertices = vertices;

            // UV情報（テクスチャのマッピング）
            mesh.uv = sprite.uv;

            // 三角形インデックス（スプライトのポリゴン情報）
            mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);

            return mesh;
        }
    }
}

