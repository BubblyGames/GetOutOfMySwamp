

### Get out of my swamp!



Versión 0.1




## 1. Introducción


### 1.1 Descripción breve del concepto

Tower defense 3D en un mapa cúbico con vista isométrica sobre ranas defendiendo su charco.


### 1.2 Descripción breve de la historia y personajes

Dos facciones de ranas se enfrentan por la conquista del mundo de una, nuestra facción. Las ranas de un estanque sucio y mugriento quieren ir a ensuciar el estanque de las otras ranas metiéndose en él con las patas llenas de barro. Las ranas del charco limpio las expulsarán con todos los recursos al alcance de sus ancas.


### 1.3 Propósito, público objetivo y plataformas

Se desarrolla para navegadores web y móviles y es para todos los públicos. El objetivo/propósito del juego es defender tu estanque de la invasión.


## 2. Monetización


### 2.1 Tipo de modelo de monetización

El modelo de monetización dependerá del desempeño económico del producto una vez lanzado al mercado. Se han estructurado 3 posibles casos: ruta optimista, ruta normal, ruta pesimista.

- Optimista:

![](https://lh3.googleusercontent.com/NDok-owYDmq-nwNgygDbRXiC_imSoQfDmGG--ZBRdhj06D-Gra9gRHcpBK-UgUpN9wqcpNrYbVjkmJv8gvRJvbv_m8-sO3VaA3N30WNo2yMRoYWLCQ1BDHIchIngL1WyXdZhekcp=s0)

- Normal:

![](https://lh5.googleusercontent.com/yLeVvIKrbgzTepkci_sdpZwFOD4xHR-sfVYYtzEeKjQ6LuV9q3_To2p_3tYslYGGOk1IOkCN2wDLkIdDePRQswpZftIUGr7SQZNl6p8MkT2oxJ1U8Ec13ciUD6yZvwZeUkitSFc6=s0)

- Pesimista:

![](https://lh6.googleusercontent.com/qtqYHgtjQsrfuDW2jzYMS6uORjEzjM1dH7jmhMES1awgUNY3AevEl5UOzc_10r92SlEo8O1f5x5UMYijkVQKC_15Q1pSoWWwYCZl0PBZe-dqzdw0vG6RoCIt5AK4DzaX2rLJqLVC=s0)


### 2.2 Tablas de productos y precios


## 3. Planificación y Costes


### 3.1 El equipo humano

El equipo está formado por 6 integrantes:

Gonzalo Lobo: Scrum Master

Marta Vidal: Artista

Alberto Garcia: Diseñador

Daniel Marti,Jose Márquez y Adrian Ramirez: Programadores


### 3.2 Estimación temporal del desarrollo

El tiempo de desarrollo para las distintas etapas del proyecto se estima en:

- Alfa: 4 semanas
- Beta: 3 semanas
- Pruebas Beta: 4 días
- Gold: ~2 semanas
- Presentación: 1 semana


### 3.3 Costes asociados


## 4. Mecánicas de Juego y Elementos de Juego


### 4.1 Descripción detallada del concepto de juego

Es un tower defense 3D con vista isométrica en un mapa cúbico. El origen de los enemigos y la meta están en caras opuestas y tienen que recorrer una serie de caminos pasando por el resto de caras para llegar al final. Los caminos serán semi procedurales, así que el jugador tendrá en primera instancia que familiarizarse con el mapa que le tocó para defenderlo adecuadamente. Los enemigos aparecen por oleadas con una dificultad que aumenta progresivamente, tanto por un aumento de número como por diversidad de tipos de enemigos. Defender satisfactoriamente tiene una recompensa económica para poder seguir expandiendo las defensas y mejorar las ya existentes. Todas las torres aparecen disponibles desde un inicio y el único factor limitante será su precio. Si una rana enemiga consigue llegar al final se reduce un punto de vida hasta que llegue a 0 y se acabe la partida.


### 4.2 Descripción detallada de las mecánicas de juego


#### 4.2.1 Victoria y derrota

Para ganar la partida se ha de defender un número determinado de oleadas y con la vida igual o superior a 1. El número de oleadas defendidas y por defender estará en pantalla al inicio de cada ronda. Si llegan a la meta un número de enemigos igual a la cantidad de vidas la partida se acaba.


#### 4.2.2 Mapa

El mapa tiene forma de cubo, con sus 6 caras siendo terreno jugable. Las caras visibles serán de 1 a 3, dependiendo de la rotación del cubo que elija el jugador. El cubo se puede rotar en cualquier momento de la partida menos en el menú de construcción.

En el mapa hay tres tipos de casillas: Casillas de camino, por las que los enemigos transitan y se pueden poner trampas; Casillas de terreno construible, donde se podrá poner cualquier tipo de torre; Y casillas obstáculo, en las que no puede haber camino ni estructuras.


#### 4.2.3 Economía

El juego comienza con una cantidad determinada de monedas para poner las torres iniciales. Una vez iniciada la partida, la fuente de ingresos será la derrota de los enemigos. El dinero conseguido se puede gastar en construir más defensas o mejorar las ya existentes.

#### 4.2.4 Defensas y enemigos

Existen dos tipos de defensas, las torres y las trampas. Las torres tienen vida, alcance, daño, velocidad de ataque y éstos pueden ser individuales o en área y tener efectos de estado sobre los enemigos. Solo se pueden construir adyacentes al camino de los enemigos u otra torre previamente construida. Además, tienen dos niveles de mejoras para aumentar sus estadísticas. Luego están las trampas, las cuales se colocan en el camino del enemigo. Tienen daño, durabilidad y pueden tener efecto.

Por otro lado están los enemigos, con características de vida, velocidad de movimiento y opcionales como la capacidad de volar o dañar las torres. Aparecen por oleadas y aumentan en número conforme avanzas en la partida. Tienen diferentes roles en función de sus estadísticas: 
- Base, unidad más común, estadísticas medias, carne de cañón
- Tanque, una gran cantidad de vida y velocidad de movimiento reducida
- Voladora, como su nombre indica, no van por el suelo ergo no son susceptibles a ciertos tipo de torres o trampas_y siguen una ruta diferente a la meta
- Horda, aparecen en mucha cantidad, con una velocidad de movimiento alta y vida bastante baja
- Por último, especialista, tiene habilidades únicas como dañar a las torres.


### 4.3 Controles

El jugador durante la partida tendrá dos tareas principales, vigilar el mapa para saber el estado de la partida y colocar defensas. Para vigilar el mapa podrá rotarlo con click (pulsar en el caso de móvil) y arrastrar. Luego para poner las torres será exactamente el mismo input, click y arrastrar desde la tarjeta de la torre hasta la posición en la que desee ponerla.


### 4.4 Niveles y misiones

El juego cuenta con 2 niveles por el momento. La misión en ambos niveles es defender tu estanque de las tropas enemigas.

Los niveles se conforman de grids y hay distintas variantes de las celdas. Los niveles son semi procedurales: los niveles se crean mediante seed, pero a medida que se progresa su dificultad va aumentando.


### 4.5 Torres y trampas

| **TORRES**     | ALCANCE   | DAÑO    | OBJETIVO          | VELOCIDAD DE ATAQUE | EFECTO                            |
| -------------- | --------- | ------- | ----------------- | ------------------- | --------------------------------- |
| TORRE PESADA   | Largo     | Elevado | ÁreaTT\*          | Lento               | \-                                |
| TORRE BÁSICA   | Medio     | Medio   | IndividualTA-TT\* | Medio               | \-                                |
| TORRE PSÍQUICA | Corto     | Bajo    | IndividualTT\*    | Medio               | El enemigo retrocede              |
| TORRE ACUÁTICA | Medio     | Bajo    | ÁreaTA-TT\*       | Rápido              | Prioriza ataque a enemigos aéreos |
| **TRAMPAS**    |           |         |                   |                     |                                   |
| TRAMPA PANTANO | 1 Casilla | \-      | ÁreaTT\*          | \-                  | Ralentiza                         |

\*TT: Ataques desde Tierra a objetivos Terrestres.
TA: Ataques desde Tierra a objetivos Aéreos.


## 5. Trasfondo


### 5.1 Descripción detallada de la historia y la trama

Dos sociedades de ranitas viven cada una en un estanque de un cubo espacial en paz y armonía. Pero una de las sociedades ha ensuciado su estanque hasta tal punto de volverlo tóxico e inhabitable, lanzando una invasión al estanque de las ranas limpias para poder encontrar un nuevo lugar donde vivir.

Las ranas limpias, ante esta situación, deben defender su estanque de la invasión de las ranas sucias.


### 5.2 Personajes

Hay 2 personajes característicos en el juego:el líder de las ranas limpias y el líder de las ranas sucias.


### 5.3 Entornos y lugares

Al haber 3 niveles jugables hay 3 entornos o biomas: pantano, nieve y ciudad.

- Pantano: el estanque de las ranas limpias es un estanque limpio, vívido y con nenúfares mientras que el estanque de las ranas sucias es oscuro, con lodo y fango.
- Ciudad: el estanque de las ranas limpias se encuentra en un parque, con bancos y papeleras; mientras que las ranas sucias vienen de las alcantarillas.
- Nieve: tanto el estanque de las ranas limpias como el de las ranas sucias son estanques normales en una montaña o un bosque, pero el de las ranas sucias es más oscuro.


## 


## 6. Arte


### 6.1 Estética general del juego

El apartado visual consistirá en modelos voxel, tanto para el escenario como para las torres y los enemigos.


### 6.2 Apartado visual

Existen distintos climas/biomas para los niveles.


### 6.3 Música


### 6.4 Ambiente sonoro


## 


## 7. Interfaz


### 7.1 Diseños básicos de los menús


### 7.2 Diagrama de flujo


## 


## 8. Hoja de ruta del desarrollo


### 8.1 Hito 1: Alfa

- Motor Voxel que genere de manera procedural el cubo, los caminos y los obstáculos.
- Funcionalidades básicas de juego (IA, vida y ataque).
- Juego responsive.
- Torres y enemigos definidos, con concepts y algún modelo de ellos.
- Campaña de marketing iniciada (redes sociales preparadas).


### 8.2 Hito 2: Beta


### 8.3 Hito 3: Gold


### 8.4 Fecha de lanzamiento

  
