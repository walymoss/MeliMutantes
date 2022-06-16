# MeliMutantes!

MeliMutantes es un servicio web implementado como Web API por medio de .Net Core 3.1. El servicio se encuentra desplegado por medio de una **AppService** en Azure, sin embargo, por motivos de seguridad se implementó un **API Gateway** como puente entre el cliente y el backend. Esto con el fin de evitar algún colapso de la API y tener acceso a tracking en tiempo real. Adicionalmente, también se implementó un servicio de **base de datos SQL en Azure**.
![Diagrama API Management Service](https://www.optiv.com/sites/default/files/inline-images/azure_api_1.png)

# Operaciones

 1. **Mutant**: Operación que sirve para validar si una cadena de ADN (matriz cuadrada) corresponde a un mutante (respuesta 200 - OK) o a un humano (respuesta 403 - Forbidden). Si se ingresan datos erróneos, por ejemplo una matriz no cuadrada, el servicio retornará error 400 (Bad Request). URL de consumo: https://apimanagementservicewaly.azure-api.net/mutant
 2. **Stats**: Operación que cuenta cantidad de mutantes, cantidad de humanos y un ratio o promedio entre ambos valores. URL de consumo: https://apimanagementservicewaly.azure-api.net/stats

# Código

Proyecto Web API .Net Core 3.1.

 1. **Sequence** Entidad que interpreta la secuencia de ADN.
 2. **Response** Entidad respuesta del POST que recibe la secuencia
 3. **Stats** Entidad que interpreta las transacciones de la BD: Humano o Mutante.
 4. **StatsResponse** Entidad respuesta que cuenta Humanos, Mutantes y Ratio
 5. **MutantController** Controlador o servicio que tiene la lógica de negocio para determinar si una secuencia corresponde a un humano o a un mutante
 6. **StatsController** Controlador o servicio que se conecta con la BD para guardar las transacciones y retornar cantidad de Humanos y Mutantes


# Pruebas Unitarias
Proyecto XUnit .Net Core 3.1.

 1. **TestMutant** Clase con pruebas unitarias para las operaciones del controlador de mutantes
 2. **TestStats** Clase con pruebas unitarias para las operaciones del controlador de stats
