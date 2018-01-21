/// Taken from http://wiki.unity3d.com/index.php?title=DepthMask 

Shader "Masked/Mask"
{
	SubShader
    {
		// Render the mask before regular geometry
 
		Tags {"Queue" = "Geometry-500" }
 
		// Don't draw in the RGBA channels; just the depth buffer
 
		ColorMask 0
		ZWrite On
 
		// Do nothing specific in the pass:
 
		Pass {}
	}
}