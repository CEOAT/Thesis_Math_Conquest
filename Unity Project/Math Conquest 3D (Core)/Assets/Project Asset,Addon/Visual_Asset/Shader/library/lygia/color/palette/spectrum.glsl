/*
original_author: Martijn Steinrucken
description: Spectrum Response Function https://www.shadertoy.com/view/wlSBzD
use: <vec3> spectrum(<float> value [, <float> blur])
*/

#ifndef FNC_SPECTRUM
#define FNC_SPECTRUM
vec3 spectrum(float x) {
    return  (vec3( 1.220023e0,-1.933277e0, 1.623776e0) +
            (vec3(-2.965000e1, 6.806567e1,-3.606269e1) +
            (vec3( 5.451365e2,-7.921759e2, 6.966892e2) +
            (vec3(-4.121053e3, 4.432167e3,-4.463157e3) +
            (vec3( 1.501655e4,-1.264621e4, 1.375260e4) +
            (vec3(-2.904744e4, 1.969591e4,-2.330431e4) +
            (vec3( 3.068214e4,-1.698411e4, 2.229810e4) +
            (vec3(-1.675434e4, 7.594470e3,-1.131826e4) +
             vec3( 3.707437e3,-1.366175e3, 2.372779e3)
            *x)*x)*x)*x)*x)*x)*x)*x)*x;
}

vec3 spectrum(float x, float blur) {
    vec4 a = vec4(  1.,   .61,   .78,  .09),
         o = vec4(-.57, -.404, -.176, -.14),
         f = vec4(223.,  165.,  321., 764.) / blur,
         c = a*pow(cos(x + o), f);
    c.r += c.w;
    return c.rgb;
}
#endif