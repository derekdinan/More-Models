using System;
using ClassicalSharp.Entities;
using ClassicalSharp.GraphicsAPI;
using ClassicalSharp.Textures;
using OpenTK;
using BlockID = System.UInt16;

namespace ClassicalSharp.Model
{
    class HoldingModel : HumanoidModel
    {
        public HoldingModel(Game game) : base(game) { MaxScale = (BlockInfo.Count + 2f); }

        public override void RecalcProperties(Entity p)
        {
            float scale = (float)Math.Floor(p.ModelScale.X);
            if (scale >= 2f)
            {
                BlockID block = (BlockID)((int)scale - 2);
                if (block < BlockInfo.Count)
                {
                    p.ModelBlock = block;
                }
                else
                {                    
                    p.ModelBlock = Block.Air;
                }
            }
            p.ModelScale = Vector3.One;
        }

        /*public override void CreateParts()
        {
            vertices = new ModelVertex[boxVertices * numParts];

            torso = BuildBox(MakeBoxBounds(-4, 12, -2, 4, 24, 2).TexOrigin(16, 16));

            head     = BuildBox(MakeBoxBounds(-4, 24, -4, 4, 32, 4) .TexOrigin(0, 0)  .RotOrigin(0, 24, 0));
            leftLeg  = BuildBox(MakeBoxBounds(0, 0, -2, -4, 12, 2)  .TexOrigin(0, 16) .RotOrigin(-2, 12, 0));
            rightLeg = BuildBox(MakeBoxBounds(0, 0, -2, 4, 12, 2)   .TexOrigin(0, 16) .RotOrigin(2, 12, 0));
            leftArm  = BuildBox(MakeBoxBounds(-4, 12, -2, -8, 24, 2).TexOrigin(40, 16).RotOrigin(-4, 22, 0));
            rightArm = BuildBox(MakeBoxBounds(4, 12, -2, 8, 24, 2)  .TexOrigin(40, 16).RotOrigin(4, 22, 0));

            hat = BuildBox(MakeBoxBounds(-4, 24, -4, 4, 32, 4).TexOrigin(32, 0).RotOrigin(0, 24, 0).Expand(0.5f));
        }*/

        //public override float NameYOffset { get { return 2.075f; } }

        //public override float GetEyeY(Entity entity) { return 26f / 16f; }

        /*public override Vector3 CollisionSize
        {
            get { return new Vector3(8f / 16f + 0.6f / 16f, 28.1f / 16f, 8f / 16f + 0.6f / 16f); }
        }

        public override AABB PickingBounds
        {
            get { return new AABB(-4f / 16f, 0f, -4f / 16f, 4f / 16f, 32f / 16f, 4f / 16f); }
        }*/

        public override void DrawModel(Entity p)
        {
            ModelSet model = p.SkinType == SkinType.Type64x64Slim ? SetSlim : (p.SkinType == SkinType.Type64x64 ? Set64 : Set);

            Rotate = RotateOrder.XZY;

            float handBob = (float)Math.Sin(p.anim.walkTime * 2f) * p.anim.swing * (float)Math.PI / 16f;
            float handIdle = p.anim.rightArmX * (1f - p.anim.swing);

            game.Graphics.BindTexture(GetTexture(p));
            game.Graphics.AlphaTest = false;

            DrawPart(model.Torso);

            DrawRotate(-p.HeadXRadians, 0f, 0f, model.Head, true);

            DrawRotate(p.anim.leftLegX, 0f, p.anim.leftLegZ, model.LeftLeg, false);
            DrawRotate(p.anim.rightLegX, 0f, p.anim.rightLegZ, model.RightLeg, false);
            DrawRotate((float)Math.PI / 3f + handBob + handIdle, (handBob + handIdle) * -2f / 3f, (float)Math.PI / 8f, model.LeftArm, false);
            DrawRotate((float)Math.PI / 3f + handBob + handIdle, (handBob + handIdle) * 2f / 3f, (float)Math.PI / -8f, model.RightArm, false);

            UpdateVB();

            game.Graphics.AlphaTest = true;
            index = 0;

            if (p.SkinType != SkinType.Type64x32)
            {
                DrawPart(model.TorsoLayer);

                DrawRotate(p.anim.leftLegX, 0f, p.anim.leftLegZ, model.LeftLegLayer, false);
                DrawRotate(p.anim.rightLegX, 0f, p.anim.rightLegZ, model.RightLegLayer, false);
                DrawRotate((float)Math.PI / 3f + handBob + handIdle, (handBob + handIdle) * -2f / 3f, (float)Math.PI / 8f, model.LeftArmLayer, false);
                DrawRotate((float)Math.PI / 3f + handBob + handIdle, (handBob + handIdle) * 2f / 3f, (float)Math.PI / -8f, model.RightArmLayer, false);
            }
            DrawRotate(-p.HeadXRadians, 0f, 0f, model.Hat, true);

            UpdateVB();
            index = 0;
            //block = p.ModelBlock;
            //RecalcProperties(p);
            if (BlockInfo.Draw[p.ModelBlock] != DrawType.Gas)
            {
                if (BlockInfo.FullBright[p.ModelBlock])
                {
                    for (int i = 0; i < cols.Length; i++)
                    {
                        cols[i] = FastColour.WhitePacked;
                    }
                }
                DrawBlockTransform(p, 0f, ((float)Math.PI / 3f + handBob + handIdle) * 10f / 16f + 8f / 16f, -9f / 16f, 0.5f);
            }

            
        }
        private void DrawBlockTransform(Entity p, float dispX, float dispY, float dispZ, float scale)
        {
            //Used for?
            int lastTexIndex = -1;
            bool sprite = BlockInfo.Draw[p.ModelBlock] == DrawType.Sprite;
            //DrawParts(sprite, p.ModelBlock, lastTexIndex);
            if (sprite)
            {
                SpriteXQuad(false, false, p.ModelBlock, ref lastTexIndex, game.ModelCache, dispX, dispY, dispZ, scale);
                SpriteXQuad(false, true, p.ModelBlock, ref lastTexIndex, game.ModelCache, dispX, dispY, dispZ, scale);
                SpriteZQuad(false, false, p.ModelBlock, ref lastTexIndex, game.ModelCache, dispX, dispY, dispZ, scale);
                SpriteZQuad(false, true, p.ModelBlock, ref lastTexIndex, game.ModelCache, dispX, dispY, dispZ, scale);

                SpriteZQuad(true, false, p.ModelBlock, ref lastTexIndex, game.ModelCache, dispX, dispY, dispZ, scale);
                SpriteZQuad(true, true, p.ModelBlock, ref lastTexIndex, game.ModelCache, dispX, dispY, dispZ, scale);
                SpriteXQuad(true, false, p.ModelBlock, ref lastTexIndex, game.ModelCache, dispX, dispY, dispZ, scale);
                SpriteXQuad(true, true, p.ModelBlock, ref lastTexIndex, game.ModelCache, dispX, dispY, dispZ, scale);
            }
            else
            {
                CuboidDrawer drawer = new CuboidDrawer();
                ModelCache cache = game.ModelCache;

                drawer.minBB = BlockInfo.MinBB[p.ModelBlock]; drawer.minBB.Y = 1 - drawer.minBB.Y;
                drawer.maxBB = BlockInfo.MaxBB[p.ModelBlock]; drawer.maxBB.Y = 1 - drawer.maxBB.Y;

                Vector3 min = BlockInfo.RenderMinBB[p.ModelBlock];
                Vector3 max = BlockInfo.RenderMaxBB[p.ModelBlock];
                drawer.x1 = (min.X - 0.5f) * scale + dispX; drawer.y1 = min.Y * scale + dispY; drawer.z1 = (min.Z - 0.5f) * scale + dispZ;
                drawer.x2 = (max.X - 0.5f) * scale + dispX; drawer.y2 = max.Y * scale + dispY; drawer.z2 = (max.Z - 0.5f) * scale + dispZ;

                drawer.Tinted = BlockInfo.Tinted[p.ModelBlock];
                drawer.TintColour = BlockInfo.FogColour[p.ModelBlock];

                drawer.Bottom(1, cols[1], GetTex(Side.Bottom, p.ModelBlock, ref lastTexIndex), cache.vertices, ref index);
                drawer.Front(1, cols[3], GetTex(Side.Front, p.ModelBlock, ref lastTexIndex), cache.vertices, ref index);
                drawer.Right(1, cols[5], GetTex(Side.Right, p.ModelBlock, ref lastTexIndex), cache.vertices, ref index);
                drawer.Back(1, cols[2], GetTex(Side.Back, p.ModelBlock, ref lastTexIndex), cache.vertices, ref index);
                drawer.Left(1, cols[4], GetTex(Side.Left, p.ModelBlock, ref lastTexIndex), cache.vertices, ref index);
                drawer.Top(1, cols[0], GetTex(Side.Top, p.ModelBlock, ref lastTexIndex), cache.vertices, ref index);
            }

            if (index == 0) return;

            if (sprite) game.Graphics.FaceCulling = true;
            lastTexIndex = texIndex;
            if (lastTexIndex != -1)
            {
                game.Graphics.BindTexture(Atlas1D.TexIds[lastTexIndex]);
                UpdateVB();
            }

            lastTexIndex = texIndex;
            index = 0;
            if (sprite) game.Graphics.FaceCulling = false;
        }

        int GetTex(int side, BlockID block, ref int lastTexIndex)
        {
            int texLoc = BlockInfo.GetTextureLoc(block, side);
            texIndex = texLoc / Atlas1D.TilesPerAtlas;

            if (lastTexIndex != texIndex)
            {
                if (lastTexIndex != -1)
                {
                    game.Graphics.BindTexture(Atlas1D.TexIds[lastTexIndex]);
                    UpdateVB();
                }

                lastTexIndex = texIndex;
                index = 0;

            }
            return texLoc;
        }

        void SpriteXQuad(bool firstPart, bool mirror, BlockID block, ref int lastTexIndex, ModelCache cache, float dispX, float dispY, float dispZ, float scale)
        {
            // Get the texture location
            int texLoc = BlockInfo.GetTextureLoc(block, Side.Right);

            TextureRec rec = Atlas1D.GetTexRec(texLoc, 1, out texIndex);
            if (lastTexIndex != texIndex)
            {
                if (lastTexIndex != -1)
                {
                    game.Graphics.BindTexture(Atlas1D.TexIds[lastTexIndex]);
                    UpdateVB();
                }

                lastTexIndex = texIndex;
                index = 0;
            }
            int col = cols[0];
            if (BlockInfo.Tinted[block])
            {
                col = Utils.Tint(col, BlockInfo.FogColour[block]);
            }

            float x1 = 0, x2 = 0, z1 = 0, z2 = 0;
            if (firstPart)
            {
                if (mirror) { rec.U2 = 0.5f; x2 = -5.5f / 16; z2 = 5.5f / 16; }
                else { rec.U1 = 0.5f; x1 = -5.5f / 16; z1 = 5.5f / 16; }
            }
            else
            {
                if (mirror) { rec.U1 = 0.5f; x1 = 5.5f / 16; z1 = -5.5f / 16; }
                else { rec.U2 = 0.5f; x2 = 5.5f / 16; z2 = -5.5f / 16; }
            }

            cache.vertices[index++] = new VertexP3fT2fC4b(x1 * scale + dispX, dispY, z1 * scale + dispZ, rec.U2, rec.V2, col);
            cache.vertices[index++] = new VertexP3fT2fC4b(x1 * scale + dispX, scale + dispY, z1 * scale + dispZ, rec.U2, rec.V1, col);
            cache.vertices[index++] = new VertexP3fT2fC4b(x2 * scale + dispX, scale + dispY, z2 * scale + dispZ, rec.U1, rec.V1, col);
            cache.vertices[index++] = new VertexP3fT2fC4b(x2 * scale + dispX, dispY, z2 * scale + dispZ, rec.U1, rec.V2, col);
        }

        void SpriteZQuad(bool firstPart, bool mirror, BlockID block, ref int lastTexIndex, ModelCache cache, float dispX, float dispY, float dispZ, float scale)
        {
            int texLoc = BlockInfo.GetTextureLoc(block, Side.Back);
            TextureRec rec = Atlas1D.GetTexRec(texLoc, 1, out texIndex);
            if (lastTexIndex != texIndex)
            {
                if (lastTexIndex != -1)
                {
                    game.Graphics.BindTexture(Atlas1D.TexIds[lastTexIndex]);
                    UpdateVB();
                }

                lastTexIndex = texIndex;
                index = 0;
            }
            int col = cols[0];
            if (BlockInfo.Tinted[block])
            {
                col = Utils.Tint(col, BlockInfo.FogColour[block]);
            }

            float p1 = 0, p2 = 0;
            if (firstPart)
            { // Need to break into two quads for when drawing a sprite model in hand.
                if (mirror) { rec.U1 = 0.5f; p1 = -5.5f / 16; }
                else { rec.U2 = 0.5f; p2 = -5.5f / 16; }
            }
            else
            {
                if (mirror) { rec.U2 = 0.5f; p2 = 5.5f / 16; }
                else { rec.U1 = 0.5f; p1 = 5.5f / 16; }
            }

            cache.vertices[index++] = new VertexP3fT2fC4b(p1 * scale + dispX, dispY, p1 * scale + dispZ, rec.U2, rec.V2, col);
            cache.vertices[index++] = new VertexP3fT2fC4b(p1 * scale + dispX, scale + dispY, p1 * scale + dispZ, rec.U2, rec.V1, col);
            cache.vertices[index++] = new VertexP3fT2fC4b(p2 * scale + dispX, scale + dispY, p2 * scale + dispZ, rec.U1, rec.V1, col);
            cache.vertices[index++] = new VertexP3fT2fC4b(p2 * scale + dispX, dispY, p2 * scale + dispZ, rec.U1, rec.V2, col);
        }
    }
}