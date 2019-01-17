#include "Common.h"
struct ModelPart base, leftLegFront, rightLegFront, leftLegBack, rightLegBack;

void TableModel_MakeParts(void) {
	// all the parts have a TexOrigin of 0,0
	struct BoxDesc box_base   = { 0,0, BoxDesc_Box(-8,16,-8,  8,18, 8) };
	struct BoxDesc box_lFront = { 0,0, BoxDesc_Box(-6,16,-6, -8, 0,-8) }; 
	struct BoxDesc box_rFront = { 0,0, BoxDesc_Box( 8,16,-6,  6, 0,-8) }; 
	struct BoxDesc box_lBack  = { 0,0, BoxDesc_Box(-6,16, 8, -8, 0, 6) }; 
	struct BoxDesc box_rBack  = { 0,0, BoxDesc_Box( 8,16, 6,  6, 0, 8) };
	
	BoxDesc_BuildBox(&base,          &box_base);
	BoxDesc_BuildBox(&leftLegFront,  &box_lFront);
	BoxDesc_BuildBox(&rightLegFront, &box_rFront);
	BoxDesc_BuildBox(&leftLegBack,   &box_lBack);
	BoxDesc_BuildBox(&rightLegBack,  &box_rBack);
}

void TableModel_Draw(struct Entity* entity) {
	Model_ApplyTexture(entity);
	Models.uScale = 1/16.0f; Models.vScale = 1/16.0f;

	Model_DrawPart(&base);
	Model_DrawPart(&leftLegFront);
	Model_DrawPart(&rightLegFront);
	Model_DrawPart(&leftLegBack);
	Model_DrawPart(&rightLegBack);

	Model_UpdateVB();
}

float TableModel_GetNameY(struct Entity* e) { return 1.50f; }
float TableModel_GetEyeY(struct Entity* e)  { return 1.25f; }
void TableModel_GetSize(struct Entity* e)   { _SetSize(14,14,14); }
void TableModel_GetBounds(struct Entity* e) { _SetBounds(-5,0,14, 5,16,9); }

static struct ModelVertex vertices[MODEL_BOX_VERTICES * 5];
static struct Model model = {
	"table", vertices, &wood_tex,
	TableModel_MakeParts, TableModel_Draw,
	TableModel_GetNameY,  TableModel_GetEyeY,
	TableModel_GetSize,   TableModel_GetBounds
};

struct Model* TableModel_GetInstance(void) {
	Model_Init(&model);
	return &model;
}