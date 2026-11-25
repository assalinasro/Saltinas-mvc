# Saltinas — Tienda de ropa (ASP.NET Core MVC)

Tienda de ropa femenina inspirada en la moda de los años 2000. Proyecto individual de Programación Web
(ASP.NET Core MVC + C# + Razor + Bootstrap + JavaScript).

## Cómo ejecutarlo

1. Copia tus carpetas originales dentro de `wwwroot/`:
   - `Imagenes/` (jean1.jpeg, top1.jpeg, ...) → `wwwroot/Imagenes/`
   - `Videos/` (tomar-medidas.mp4) → `wwwroot/Videos/`
2. En la terminal de VS Code, dentro de esta carpeta:

```bash
dotnet --version     # si no empieza por 8, cambia <TargetFramework> en SaltinasMvc.csproj (net9.0, net10.0...)
dotnet run
```

3. Abre http://localhost:5080

## Estructura y responsabilidades (MVC)

| Carpeta / archivo | Responsabilidad |
|---|---|
| `Models/` | Entidades y modelos: `Producto`, `Cliente`, `RegistroViewModel` (con validaciones), `TallaViewModel`, etc. Representan los datos. |
| `Views/` | Vistas Razor (`.cshtml`) con HTML + Bootstrap. Solo muestran datos; `_Layout` es la plantilla común. |
| `Controllers/` | Reciben la solicitud, usan los servicios, arman el modelo y eligen la vista: `Home`, `Catalogo`, `Tallas`, `Registro`. |
| `Services/` | Lógica de negocio en C#: catálogo, cálculo de talla, registro de clientas (hash de contraseña). |
| `wwwroot/` | Archivos estáticos: `css/site.css`, `js/site.js` (carrito), `Imagenes/`, `Videos/`. |
| `Program.cs` | Configura el servidor, la inyección de dependencias y el enrutamiento. |

## Funcionalidades

- **Catálogo** (`GET /Catalogo?categoria=Vestidos`): filtra por categoría en el servidor.
- **Carrito**: JavaScript + `localStorage`, pago por WhatsApp.
- **Guía de tallas** (`GET /Tallas` muestra el formulario, `POST /Tallas` calcula la talla en C#).
- **Registro** (`GET /Registro` muestra el formulario, `POST /Registro` valida con `ModelState`, guarda y redirige a `/Registro/Gracias`).

## Recorrido de una solicitud (para el video)

1. El navegador pide `GET /Catalogo?categoria=Vestidos`.
2. Kestrel recibe la solicitud y el **middleware** la pasa por el enrutamiento (`MapControllerRoute`).
3. La ruta `{controller}/{action}/{id?}` selecciona `CatalogoController.Index("Vestidos")`.
4. El controlador pide los datos a `CatalogoService`, los pone en un `CatalogoViewModel` y llama `View(modelo)`.
5. El motor **Razor** combina `Views/Catalogo/Index.cshtml` con el `_Layout` y genera el HTML.
6. El HTML vuelve al navegador como respuesta; el navegador pide después `site.css`, `site.js` e imágenes a `wwwroot`.

## Git y GitHub

```bash
git init
git branch -M main
git add .
git commit -m "Crea proyecto ASP.NET Core MVC base"
```

Sugerencia de commits (uno por avance, así el historial se ve ordenado en el video):

```bash
git add Models Services && git commit -m "Agrega modelos y servicios (catalogo, clientes, tallas)"
git add Controllers && git commit -m "Agrega controladores Home, Catalogo, Tallas y Registro"
git add Views && git commit -m "Agrega vistas Razor con Bootstrap"
git add wwwroot && git commit -m "Agrega estilos, carrito en JavaScript y recursos"
git add Dockerfile README.md && git commit -m "Agrega Dockerfile y documentacion"
```

Publicar en GitHub (crea antes un repositorio vacío en github.com):

```bash
git remote add origin https://github.com/TU_USUARIO/saltinas-mvc.git
git push -u origin main
```

- **Commit**: guarda una "foto" de los cambios en tu repositorio local, con un mensaje.
- **Push**: sube tus commits locales al repositorio remoto (GitHub).
- **Pull**: trae a tu computador los commits nuevos del repositorio remoto.

## Publicación (Render, con Docker)

1. Entra a https://render.com e inicia sesión con GitHub.
2. **New → Web Service** → elige tu repositorio `saltinas-mvc`.
3. **Runtime: Docker** (detecta el `Dockerfile` solo). Plan **Free**.
4. **Create Web Service**. Al terminar, Render te da una URL pública `https://...onrender.com`.

Cada `git push` a `main` vuelve a desplegar automáticamente. En el plan gratuito la primera carga después de un rato
inactivo puede tardar ~1 minuto: abre la URL antes de grabar el video.

> Nota: los registros de clientas se guardan en memoria; se reinician cuando el servidor se reinicia.
> Para una versión con base de datos se usaría Entity Framework Core.

## Guion para el video (Teams)

1. App funcionando en local → catálogo, filtro, carrito, calculadora de tallas y registro (prueba un error y un registro exitoso).
2. Estructura en VS Code: explicar Models, Views, Controllers, Services, wwwroot.
3. Código: `Producto` → `CatalogoService` → `CatalogoController` → `Views/Catalogo/Index.cshtml`.
4. Formularios: `RegistroController` GET vs POST, `ModelState`, DataAnnotations y redirección a `Gracias`.
5. GitHub: historial de commits; explicar commit, push y pull.
6. URL pública funcionando.
7. Recorrido de la solicitud (sección anterior).
