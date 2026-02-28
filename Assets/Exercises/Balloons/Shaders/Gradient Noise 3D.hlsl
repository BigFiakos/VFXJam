
void Hash_Tchou_3_3_uint_Custom(uint3 v, out uint3 o)
{
    // ~15 alu (3 mul)
    v.x ^= 1103515245U;
    v.y ^= v.x + v.z;
    v.y = v.y * 134775813;
    v.z += v.x ^ v.y;
    v.y += v.x ^ v.z;
    v.x += v.y * v.z;
    v.x = v.x * 0x27d4eb2du;
    v.z ^= v.x << 3;
    v.y += v.z << 3;
    o = v;
}

void Hash_Tchou_3_3_float_Custom(float3 i, out float3 o)
{
    uint3 r, v = (uint3) (int3) round(i);
    Hash_Tchou_3_3_uint_Custom(v, r);
    o = (r >> 8) * (1.0 / float(0x00ffffff));
}

float3 Unity_GradientNoise_Deterministic_Dir3_float(float3 p)
{
    float3 x;
    Hash_Tchou_3_3_float_Custom(p, x);
    return normalize(frac(x) - 0.5);
}

void Unity_GradientNoise_3D_Deterministic_float(float3 UV, float3 Scale, out float Out)
{
    float3 p = UV * Scale;
    float3 ip = floor(p);
    float3 fp = frac(p);
    float2 h = float2(0, 1);
    float d000 = dot(Unity_GradientNoise_Deterministic_Dir3_float(ip + h.xxx), fp - h.xxx);
    float d010 = dot(Unity_GradientNoise_Deterministic_Dir3_float(ip + h.xyx), fp - h.xyx);
    float d100 = dot(Unity_GradientNoise_Deterministic_Dir3_float(ip + h.yxx), fp - h.yxx);
    float d110 = dot(Unity_GradientNoise_Deterministic_Dir3_float(ip + h.yyx), fp - h.yyx);
    float d001 = dot(Unity_GradientNoise_Deterministic_Dir3_float(ip + h.xxy), fp - h.xxy);
    float d011 = dot(Unity_GradientNoise_Deterministic_Dir3_float(ip + h.xyy), fp - h.xyy);
    float d101 = dot(Unity_GradientNoise_Deterministic_Dir3_float(ip + h.yxy), fp - h.yxy);
    float d111 = dot(Unity_GradientNoise_Deterministic_Dir3_float(ip + h.yyy), fp - h.yyy);
    fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
    Out = lerp(lerp(lerp(d000, d010, fp.y), lerp(d100, d110, fp.y), fp.x) + 0.5,
              lerp(lerp(d001, d011, fp.y), lerp(d101, d111, fp.y), fp.x) + 0.5, fp.z);
}