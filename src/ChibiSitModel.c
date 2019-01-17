#include "Common.h"
static struct Model* chibi;
#define SIT_OFFSET (5/16.0f)

void ChibiSitModel_MakeParts(void) { }

void ChibiSitModel_Draw(struct Entity* e) {
	e->Anim.LeftLegX =  1.5f; e->Anim.RightLegX = 1.5f;
	e->Anim.LeftLegZ = -0.1f; e->Anim.RightLegZ = 0.1f;
	Model_SetupState(chibi, e);
	chibi->Draw(e);
}

void ChibiSitModel_GetTransform(struct Entity* e, Vector3 pos, struct Matrix* m) {
	pos.Y -= SIT_OFFSET * e->ModelScale.Y;
	Entity_GetTransform(e, pos, e->ModelScale, m);
}

float ChibiSitModel_GetEyeY(struct Entity* e)  { return chibi->GetEyeY(e) - SIT_OFFSET; }
void ChibiSitModel_GetSize(struct Entity* e) {
	chibi->GetCollisionSize(e); e->Size.Y -= SIT_OFFSET;
}
void ChibiSitModel_GetBounds(struct Entity* e) {
	chibi->GetPickingBounds(e); e->ModelAABB.Max.Y -= SIT_OFFSET;
}

static struct Model model;
struct Model* ChibiSitModel_GetInstance(void) {
	// copy everything from chibi model
	String name = String_FromConst("chibi");
	chibi = Model_Get(&name);
	model = *chibi;

	model.Name         = "chibisit";
	model.MakeParts    = ChibiSitModel_MakeParts;
	model.Draw         = ChibiSitModel_Draw;
	model.GetTransform = ChibiSitModel_GetTransform;
	// TODO: check chibi sit draw arms 

	model.GetEyeY          = ChibiSitModel_GetEyeY;
	model.GetCollisionSize = ChibiSitModel_GetSize;
	model.GetPickingBounds = ChibiSitModel_GetBounds;
	return &model;
}