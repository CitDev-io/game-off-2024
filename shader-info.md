# Shader Info

Notes for safe keeping

## What I Learned About Shaders
Okay, so if we were to evaluate the scene and perspective and camera, we'd know what we're going to draw, right?

Before we send the vertexes to become pixels and the final output to the user, a Shader gets a chance to manipulate the information programatically.  

Chat gives a better nuanced description:

> Shaders operate on graphics data (like vertices and pixels) before the final image is rendered. Think of shaders as small programs that influence rendering by processing geometry (vertex shaders) or coloring pixels (fragment shaders). They run on the GPU and work on each vertex or pixel individually, allowing for real-time manipulation of visual elements. In Unity, shaders can control everything from color, texture, and lighting to complex visual effects, shaping the final look of what’s rendered on screen.


In the case of this project, we're distorting the positioning of the vertices that make up the image to give a bending effect.

## How To Configure

All materials should be using a package-provided shader and select the effect (Small World Z) that it wants to be a part of. (use ID to manage separate effects of same time). Main trick here is that its going to need a lot of vertices if we want it to be bendable. There are two 3d meshes that can be used to billboard with a bunch of superfluous pixels to make use of: See the `plane` object, it has a few packed inside.

Somewhere in the scene, there's a "controller" component that you set to your effect-and-id and modify the curvature.  Here's you set a pivot point. think of this as the perspective's transform. This will be the main character or the camera, likely.

## Tricky Bits

There is a mobile > sprite (lit) shader available, but you cannot have a SpriteRenderer use a complex mesh.  If 4 vertexes if enough, you can go the sprite route so you can hop swap the image. 4 isn't much, so it really ought to be a small image.

To render an image with many vertices for bending, create a material and set the albeddo channel to the sprite.

Anything in world space that you want to position correctly and bend along with the effect needs the shader.  TextMeshPro needs the shader on it's material.

## Getting Positioning Right
You can turn off the bending effect and work without it. Everything's just flat. Just treat it like a top-down game and let the shader take care of the curvature to make it look good. We box in the camera so it doesn't leave the square of the background. Clipping the edge would look weird.

## Effects on UI Layer
Absolutely none

## Static elements in world space
I've found that making a child of the camera is the best way to ensure it's not going to get warped by the effect (for far background)
