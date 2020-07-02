# HomeKit-Test
This is a minimal implementation of the Apple Homekit Accessory Protocol in C# that emulates an outlet device. It's implemented in a just enough to work in my limited test scenarios, so I'm not sure how it will perform in the real world. It does support the required 8 connections and 16 pairings, but proocol adherence is otherwise loose at best.
It's intended as a prototype to implement on a micro-controller, so has no-dependencies (except for a Bonjour library) and uses all 32-bit operations.
It includes homebrew implementations of SRP, ed25519, x25519 and chcha20-poly1305 which are sure are super dodgy, but they seem to work.
There are also some rough around the edges implementations of an HTTP server, multi-precision arithmetic, Montgomery reduction and exponentiation, and Barrett reduction.
Code is deliberately written not to be object-orientated (despite C# best attempts to make it look that way), also to simplify implementation on a basic IDE.
I've cleaned out most of the test code, but there are a few extra projects with tests and sample code I accumulated along the way.

Code is pretty rough, and likely worst-practice in a lot of cases, but I'm not a trained coder so I have no idea...

It seems like there are some other examples out there that are more complete, but if you want to know the bare minimum to get HAP to work, here it is.

For usage you should be able to build the project, then presuming all is well discover the device in the Apple Home app. Turning the outlet on and off operates the checkbox 
