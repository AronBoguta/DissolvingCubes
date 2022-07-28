# DissolvingCubes
Showcase of shaders, assignable input, object pool and command patterns 

This project utilizes Shader Graph to show the dissolving effect in real time, controlled by the component controller. 
Dependency Injection is mocked in order to avoid external package dependencies. Loose coupling was on of the goals of this project thanks to
controlers injecting dependencies and not depending on specific implementation but rather interfaces. A memory pool is create per object type, bound to a prefab.
A command pattern is used to allow extensible control over objects and triggering desired behaviours.

![image](https://user-images.githubusercontent.com/109797686/181630546-83db613b-8c0b-4c36-8881-d2c05f1b7424.png)
