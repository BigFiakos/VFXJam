  


void GetDiscShape_float(float2 uv, float radius, out float value)
{
    
    value = 1 - saturate(distance(uv, 0.5) * 2 / radius);
   

}