# Manual de Usuario - Repositorio Digital e Inteligencia Artificial (engin_project)

Este documento es una guía paso a paso del funcionamiento de la aplicación de escritorio **engin_project** (desarrollada en C# .NET Windows Forms). Sigue esta guía para conocer cómo operar el sistema y saber exactamente en qué puntos tomar capturas de pantalla para tu documentación o reporte.

---

## 📌 Requisitos Previos antes de Iniciar
Antes de ejecutar la aplicación, ten en cuenta el estado de los servicios:
1. **Base de Datos (PostgreSQL):** Ya está configurada en la nube (Render). No necesitas iniciar ningún gestor local (pgAdmin/dbBearer) a menos que quieras inspeccionar los datos directamente.
2. **Servicio de Inteligencia Artificial (FastAPI):** Se conecta a `http://localhost:8000`. 
   > [!IMPORTANT]
   > Para que las funciones de **Chat con IA** y **Subida rápida (Embeddings)** funcionen, debes tener corriendo la API de Python en tu máquina local en el puerto `8000`.

---

## 🚀 Guía de Uso y Pasos para Capturas de Pantalla

### Paso 1: Inicio de Sesión (LoginForm)
Al compilar y ejecutar la aplicación desde Visual Studio, aparecerá la pantalla de Login.

* **Credenciales de Acceso:**
  * **Usuario:** `admin`
  * **Contraseña:** `libre`
* **Acciones:**
  * Escribe las credenciales y haz clic en **Login**.
* 📸 **Captura sugerida:** Toma una captura de la ventana de inicio de sesión con los datos ingresados antes de hacer clic en el botón.

---

### Paso 2: Panel Principal (MainForm)
Una vez iniciada la sesión, se abrirá el panel principal que contiene un menú lateral con las opciones principales del sistema.

* **Opciones disponibles:**
  * **Usuarios (Icono de usuario):** Abre el formulario para registrar o modificar usuarios.
  * **Lista de Usuarios (Icono de lista):** Muestra el catálogo de usuarios activos.
  * **Registrar Documentos (Icono de documento):** Abre el formulario para registrar documentos manualmente.
  * **Lista de Documentos (Icono de lista de documentos):** Muestra todos los documentos registrados y permite descargarlos.
  * **Chat con IA (Botón de Chat):** Abre la ventana de chat interactivo con el asistente inteligente.
* 📸 **Captura sugerida:** Captura de la pantalla principal completa para mostrar la interfaz de bienvenida y el menú lateral interactivo.

---

### Paso 3: Gestión de Usuarios (Módulo de Usuarios)

#### A. Registro de un nuevo Usuario
1. Haz clic en el botón de **Usuarios** del menú lateral.
2. Llena los campos solicitados:
   * **Nombres / Apellidos**
   * **Usuario** (Nombre de acceso)
   * **Correo electrónico**
   * **Contraseña**
   * **Rol** (Selecciona una de las opciones del desplegable: Administrador, Operador, etc., cargados automáticamente desde la base de datos).
3. Haz clic en **Guardar**. Verás un mensaje flotante que confirma: *"Usuario registrado correctamente"*.
* 📸 **Captura sugerida:** Formulario de usuarios con los campos rellenados antes de guardar, y otra captura mostrando el cuadro de diálogo de confirmación exitosa.

#### B. Listado y Edición de Usuarios
1. Haz clic en **Lista de Usuarios** en el menú lateral.
2. Verás una tabla (DataGridView) con todos los usuarios activos recuperados en tiempo real de la base de datos en la nube.
3. Puedes hacer clic en el botón **Editar** en cualquiera de las filas para cargar los datos en el formulario y modificarlos, o en **Eliminar** para darlo de baja (cambiar su estado a falso).
* 📸 **Captura sugerida:** Captura de la tabla con los usuarios registrados en el sistema.

---

### Paso 4: Gestión de Documentos (Módulo de Archivos)

#### A. Registro de Documento Manual
1. Haz clic en **Registrar Documentos** (icono de archivo) en el menú lateral.
2. Rellena los datos:
   * **Código:** Código identificador único del documento.
   * **Título / Descripción:** Nombre descriptivo y resumen del contenido.
   * **Categoría:** Selecciona la categoría a la que pertenece.
   * **Fecha de Documento:** Fecha oficial del archivo.
   * **Archivo:** Haz clic en **Seleccionar Archivo** para elegir un PDF, PNG o JPG desde tu computadora (el sistema copiará el archivo a la ruta local `Mis Documentos/engin_project/files_received`).
3. Haz clic en **Guardar**.
* 📸 **Captura sugerida:** El formulario con los metadatos del documento y la ruta del archivo seleccionado lista para guardar.

#### B. Listado de Documentos y Descarga
1. Haz clic en **Lista de Documentos**.
2. Aparecerá el listado con los títulos, categorías y descripciones de todos los archivos guardados.
3. Al presionar el botón de acción correspondiente, podrás registrar la descarga en la bitácora del sistema.
* 📸 **Captura sugerida:** Captura de la cuadrícula de documentos activos registrados en el repositorio.

---

### Paso 5: Asistente Virtual (Chat con IA)
Esta es la interfaz de interacción inteligente con los documentos del repositorio.

1. Haz clic en el botón **Chat** en la parte inferior del menú lateral.
2. Se abrirá una nueva ventana de chat.
3. Escribe una pregunta sobre los documentos del sistema en el campo de texto (por ejemplo: *"¿Cuáles son los temas del documento de contratos?"*).
4. Presiona **Enter** o haz clic en **Enviar**.
5. El sistema consultará al backend local de FastAPI y te mostrará la respuesta en verde generada por el LLM.
* 📸 **Captura sugerida:** Ventana de Chat abierta con una conversación activa donde se vea una pregunta realizada y la respuesta devuelta por el asistente virtual.
