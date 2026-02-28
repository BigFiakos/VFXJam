

#define TEX_SIZE 64

void GetVertexCoords_float(uint vertexID, out float2 uv)
{
    uint x = vertexID % TEX_SIZE;
    uint y = vertexID / TEX_SIZE;
    
    uv = float2((x + 0.5) / TEX_SIZE, (y + 0.5) / TEX_SIZE);
}