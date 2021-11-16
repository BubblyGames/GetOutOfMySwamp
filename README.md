

### Pond Platoon
### by BubblyGames Studio



Versión 0.5




## 1. Introducción


### 1.1 Descripción breve del concepto

Tower defense 3D en un mapa cúbico con vista isométrica sobre ranas defendiendo su charco.


### 1.2 Descripción breve de la historia y personajes

En un mundo cúbico hay dos grupos de ranas en caras opuestas de éste, las ranas del estanque sucio y mugriento quieren arrebatarle el estanque de las ranas limpias porque el suyo ya no es habitable. Las ranas del charco limpio las expulsarán con todos los recursos al alcance de sus ancas.


### 1.3 Propósito, público objetivo y plataformas

Se desarrolla para navegadores web y móviles, dirigido es para todos los públicos. El objetivo del juego es defender tu estanque de la invasión de ranitas sucias.


## 2. Monetización


### 2.1 Tipo de modelo de monetización

El modelo de monetización dependerá del desempeño económico del producto una vez lanzado al mercado. Se han estructurado 3 posibles casos: ruta optimista, ruta normal, ruta pesimista.

![Ruta-Optimista 2](https://user-images.githubusercontent.com/56488179/140507385-0ab6b37b-fa93-49e6-ab92-213dce35a1aa.jpg)

![Ruta-Normal 2](https://user-images.githubusercontent.com/56488179/140507397-54ba72cd-e751-424d-b67c-791d360da57c.jpg)

![Ruta-Pesimista 2](https://user-images.githubusercontent.com/56488179/140507410-1169a49a-f06d-49c8-95d5-5bfa44343184.jpg)

Nuestro cliente se trata de una persona con una fuerte presencia del arte en su vida. Es una persona sencilla, amable, dulce y optimista, la cual quiere o la gustaría poder dedicarse al arte y vivir de ello, siendo reconocida por más gente del gremio aunque para ello debe competir contra gente con talento en una situación laboral un tanto delicada.

![nuestroMapa](https://user-images.githubusercontent.com/56488179/138941794-d059db82-28a3-4edf-8668-2a979de50dbb.png)

### 2.2 Mapa de lienzo

![lienzo_Beta](https://user-images.githubusercontent.com/56488179/140507434-c524ac65-eed2-472b-8f6d-7883491391b7.png)

### 2.3 Tablas de productos y precios


## 3. Planificación y Costes


### 3.1 El equipo humano

El equipo está formado por 6 integrantes:

Gonzalo Lobo: Scrum Master, Diseñador, Publicidad

Marta Vidal: Artista

Alberto García: Diseñador

Daniel Martí: Programador

José Márquez: Programador

Adrián Ramírez: Programador, Publicidad


### 3.2 Estimación temporal del desarrollo

El tiempo de desarrollo para las distintas etapas del proyecto se estima en:

- Alfa: 4 semanas
- Beta: 3 semanas
- Pruebas Beta: 5 días
- Gold: 2 semanas y 2 días
- Presentación: 1 semana


### 3.3 Costes asociados

Los costes del trabajo consisten en los salarios de programadores, gestores, artistas, publicistas y diseñadores.

### 3.4 Campaña de publicidad

La campaña de publicidad se va a desarrollar en 4 plataformas distintas: Twitter, Reddit, Itch.io y YouTube.
La estrategia general consiste en captar a gente a través de Reddit y Twitter para redirigirles a Itch.io, donde encontrarán más información sobre el juego. Mientras YouTube actúa como plataforma dedicada a mostrar de manera extensa el juego.
- Twitter: subir fotos de elementos de arte, videos gameplay y más elementos del juego, incluyendo en el mensaje un enlace a la página de Itch.io. Se aprovecharán hastags del mundo indie (#ScreenshotSaturday, #indiegame, #indiedev y #indiesp) para conseguir más alcance en el público adecuado.
- Reddit: lo mismo que en Twitter, pero se subirán a subreddits del mundo indie para tener más alcance. En este caso se subirán contenidos a r/IndieGaming, a r/indiegames, a r/frogs y a r/frogsandtoads.
- YouTube: subir versiones extendidas de los vídeos publicados en el resto de redes. Indicar tipo de contenido mostrado en el vídeo.
- Itch.io: subir DevBlogs hablando del desarrollo del juego en cada etapa. Comentar los aspectos más relevantes de cada equipo.

En el caso de las plataformas de Twitter, Reddit y YouTube se subirán elementos los días con más público en cada plataforma (sábados en el caso de Twitter). Para Itch.io se subirán los DevBlogs el último día de cada fase de desarrollo.

## 4. Mecánicas de Juego y Elementos de Juego


### 4.1 Descripción detallada del concepto de juego

Es un tower defense 3D con vista isométrica en un mapa cúbico. El origen de los enemigos y la meta están en caras opuestas y tienen que recorrer una serie de caminos pasando por el resto de caras para llegar al final. Los caminos serán semi procedurales, así que el jugador tendrá en primera instancia que familiarizarse con el mapa que le tocó para defenderlo adecuadamente. Los enemigos aparecen por oleadas con una dificultad que aumenta progresivamente, tanto por un aumento de número como por diversidad de tipos de enemigos. Defender satisfactoriamente tiene una recompensa económica para poder seguir expandiendo las defensas y mejorar las ya existentes. Todas las torres aparecen disponibles desde un inicio y el único factor limitante será su precio. Si una rana enemiga consigue llegar al final se reduce un punto de vida hasta que llegue a 0 y se acabe la partida.


### 4.2 Descripción detallada de las mecánicas de juego

![](https://lh4.googleusercontent.com/7_tib0ycPSkRPNOmPVR7lpDnccMY1CH9RyNUzCjInd67h-6ZX0qLFfUIJddRScMD8ORfV0BSEqDi8Iqpj8Wfii9Jr68iM_2h8t9bEcG8d3XK19vl0DOSLOA-FJglhf44LmBII0In=s1600)

#### 4.2.1 Victoria y derrota

Para ganar la partida se ha de defender un número determinado de oleadas y con la vida igual o superior a 1. El número de oleadas defendidas y por defender estará en pantalla al inicio de cada ronda. Si llegan a la meta un número de enemigos igual a la cantidad de vidas la partida se acaba.


#### 4.2.2 Mapa

El mapa tiene forma de cubo, con sus 6 caras siendo terreno jugable. Las caras visibles serán de 1 a 3, dependiendo de la rotación del cubo que elija el jugador. El cubo se puede rotar en cualquier momento de la partida menos en el menú de construcción.

En el mapa hay tres tipos de casillas: Casillas de camino, por las que los enemigos transitan y se pueden poner trampas; Casillas de terreno construible, donde se podrá poner cualquier tipo de torre; y casillas obstáculo, en las que no puede haber camino ni estructuras.


#### 4.2.3 Economía

El juego comienza con una cantidad determinada de monedas para poner las torres iniciales. Una vez iniciada la partida, la fuente de ingresos será la derrota de los enemigos y el edificio que genera monedas. El dinero conseguido se puede gastar en construir más estructuras o mejorar las ya existentes.


#### 4.2.4 Defensas y enemigos

Existen dos tipos de defensas: Las torres y las trampas. Las torres tienen vida, alcance, daño, velocidad de ataque y éstos pueden ser individuales o en área y tener efectos de estado sobre los enemigos. Se puede construir en cualquier lugar menos caminos y rocas, a excepción de la torre económica que solo se puede poner adyacente al camino. Además, tienen tres niveles de mejoras para aumentar sus estadísticas. Luego están las trampas, las cuales se colocan en el camino del enemigo, tienen daño, durabilidad y pueden tener efecto.

Por otro lado están los enemigos, con características de vida, velocidad de movimiento y opcionales como la capacidad de volar o dañar las torres. Aparecen por oleadas y aumentan en número conforme avanzas en la partida. Tienen diferentes roles en función de sus estadísticas, siendo los roles unidad básica, ligera, tanque, horda, especialista y voladora, explicadas más adelante en el apartado Enemigos.


![](https://lh6.googleusercontent.com/XyZfstCSw-DLWgamwydXli8vhxtgT-UwKJedln-d8n8b128AX2uwMAu_ZfEi14CkXv6f0j37Y27lFRzzMlJHlza6lMVwx9gM2rYwonhWBWuMDhGatPP_NvB05I83jCBoDTnug5R2=s1600)




### 4.3 Controles

El jugador durante la partida tendrá dos tareas principales, vigilar el mapa para saber el estado de la partida y colocar defensas. Para vigilar el mapa podrá rotarlo con click (pulsar en el caso de móvil) y arrastrar. Luego para poner las torres será exactamente el mismo input, click y arrastrar desde la tarjeta de la torre hasta la posición en la que desee ponerla.


### 4.4 Niveles y misiones

El juego cuenta con 4 niveles por el momento, diferenciados a nivel visual y en la generación del mapa. La misión en ambos niveles es defender tu estanque de las tropas enemigas.

Los niveles se conforman de grids y hay distintas variantes de las celdas. Los niveles son semi procedurales y se crean mediante una seed.


### 4.5 Torres y trampas

| **TORRES**     | ALCANCE      | DAÑO           | OBJETIVO          | VELOCIDAD DE ATAQUE | EFECTO                            |
| -------------- | ---------    | -------------- | ----------------- | ------------------- | --------------------------------- |
| TORRE PESADA   | Largo (7 ud) | Elevado (35 ud)| ÁreaTT\*          | Lento (0.1 ud)      | \-                                |
| TORRE BÁSICA   | Medio (5 ud) | Medio (10 ud)  | IndividualTA-TT\* | Medio (1 ud)        | \-                                |
| TORRE PSÍQUICA | Corto (3 ud) | Bajo (7 ud)    | IndividualTT\*    | Medio (1 ud)        | El enemigo se ralentiza           |
| TORRE MONTAÑA  | Medio (x ud) | Bajo (x ud)    | ÁreaTA-TT\*       | Rápido              | Prioriza ataque a enemigos aéreos |
| **TRAMPAS**    |              |                |                   |                     |                                   |
| TRAMPA PANTANO | 1 Casilla    | \-             | ÁreaTT\*          | \-                  | Ralentiza                         |

Cada defensa del jugador se puede mejorar, por separado, hasta tres veces, aumentando con estas mejoras las estadísticas de dicha defensa y haciéndola más eficaz en combate.

Nivel 1:
| **TORRES**     | ALCANCE      | DAÑO           | OBJETIVO          | VELOCIDAD DE ATAQUE | EFECTO                            | COSTE  |
| -------------- | ---------    | -------------- | ----------------- | ------------------- | --------------------------------- |--------|
| TORRE PESADA   | Largo (7 ud) | Elevado (70 ud)| ÁreaTT\*          | Lento (0.1 ud)      | \-                                | 150    |
| TORRE BÁSICA   | Medio (5 ud) | Medio (12 ud)  | IndividualTA-TT\* | Medio (1.5 ud)      | \-                                | 50     |
| TORRE PSÍQUICA | Corto (4 ud) | Bajo (8 ud)    | IndividualTT\*    | Medio (1 ud)        | El enemigo se ralentiza           | 100    |
| TORRE MONTAÑA  | Medio (x ud) | Bajo (x ud)    | ÁreaTA-TT\*       | Rápido              | Prioriza ataque a enemigos aéreos |        |
| **TRAMPAS**    |              |                |                   |                     |                                   |        |
| TRAMPA PANTANO | 1 Casilla    | \-             | ÁreaTT\*          | \-                  | Ralentiza                         |        |

Nivel 2:
| **TORRES**     | ALCANCE      | DAÑO           | OBJETIVO          | VELOCIDAD DE ATAQUE | EFECTO                            | COSTE  |
| -------------- | ---------    | -------------- | ----------------- | ------------------- | --------------------------------- |--------|
| TORRE PESADA   | Largo (12 ud)| Elevado (70 ud)| ÁreaTT\*          | Lento (0.1 ud)      | \-                                | 200    |
| TORRE BÁSICA   | Largo (8 ud) | Medio (14 ud)  | IndividualTA-TT\* | Medio (1.5 ud)      | \-                                | 100    |
| TORRE PSÍQUICA | Medio (5 ud) | Medio (9 ud)   | IndividualTT\*    | Medio (1 ud)        | El enemigo se ralentiza           | 150    |
| TORRE MONTAÑA  | Medio (x ud) | Bajo (x ud)    | ÁreaTA-TT\*       | Rápido              | Prioriza ataque a enemigos aéreos |        |
| **TRAMPAS**    |              |                |                   |                     |                                   |        |
| TRAMPA PANTANO | 1 Casilla    | \-             | ÁreaTT\*          | \-                  | Ralentiza                         |        |

Nivel 3:
| **TORRES**     | ALCANCE      | DAÑO           | OBJETIVO          | VELOCIDAD DE ATAQUE | EFECTO                            | COSTE  |
| -------------- | ---------    | -------------- | ----------------- | ------------------- | --------------------------------- |--------|
| TORRE PESADA   | Largo (12 ud)| Elevado (70 ud)| ÁreaTT\*          | Lento (0.2 ud)      | \-                                | 300    |
| TORRE BÁSICA   | Largo (10 ud)| Medio (14 ud)  | IndividualTA-TT\* | Rápido (1.75 ud)    | \-                                | 150    |
| TORRE PSÍQUICA | Medio (5 ud) | Medio (9 ud)   | IndividualTT\*    | Medio (1.5 ud)      | El enemigo se ralentiza           | 200    |
| TORRE MONTAÑA  | Medio (x ud) | Bajo (x ud)    | ÁreaTA-TT\*       | Rápido              | Prioriza ataque a enemigos aéreos |        |
| **TRAMPAS**    |              |                |                   |                     |                                   |        |
| TRAMPA PANTANO | 1 Casilla    | \-             | ÁreaTT\*          | \-                  | Ralentiza                         |        |

\*TT: Ataques desde Tierra a objetivos Terrestres.
TA: Ataques desde Tierra a objetivos Aéreos.

### 4.6 Enemigos

| **ENEMIGOS**(PONER NOMBRES) | VIDA | VELOCIDAD DE MOVIMIENTO | TIPO                 |
| --------------------------- | ---- | ----------------------- | -------------------- |
| NORMAL                      |  40  | Media                   | Terrestre - Normal   |
| LIGERA                      |      | Rápida                  | Terrestre - Ligera   |
| TANQUE                      | 250  | Lenta                   | Terrestre - Pesada   |
| HORDA                       |  25  | Media                   | Terrestre - Horda    |
| ESPECIALISTA                |      | Lenta                   | Terrestre - Especial |
| VOLADORA                    |      | Media                   | Aérea - Normal       |  

## 5. Trasfondo


### 5.1 Descripción detallada de la historia y la trama

Dos sociedades de ranitas viven cada una en un estanque de un cubo espacial en paz y armonía. Pero una de las sociedades ha ensuciado su estanque hasta tal punto de volverlo tóxico e inhabitable, lanzando una invasión al estanque de las ranas limpias para poder encontrar un nuevo lugar donde continuar sus actividades.

Las ranas limpias, ante esta situación, deben defender su estanque de la invasión de las ranas sucias.


### 5.2 Personajes

Hay 2 personajes característicos en el juego: el líder de las ranas limpias y el líder de las ranas sucias.


### 5.3 Entornos y lugares

Hay 4 niveles jugables y se han pensado en 3 entornos o biomas: pantano, nieve y ciudad.

- Pantano: el estanque de las ranas limpias es un estanque limpio, vívido y con nenúfares mientras que el estanque de las ranas sucias es oscuro, con lodo y fango.
- Ciudad: el estanque de las ranas limpias se encuentra en un parque, con bancos y papeleras; mientras que las ranas sucias vienen de las alcantarillas.
- Nieve: tanto el estanque de las ranas limpias como el de las ranas sucias son estanques normales en una montaña o un bosque, pero el de las ranas sucias es más oscuro.


## 


## 6. Arte


### 6.1 Estética general del juego

Los gráficos del juego son principalmente 3D, de estética y técnica Voxel Art, tanto para el escenario como para la representación física de defensas y enemigos. Por otro lado, todo lo relacionado con la interfaz, el HUD y otros elementos varios (cartas de personajes, información adicional, etc) son en 2D.

Con todo, la estética general del juego está basada en el minimalismo, con colores planos y suaves, es decir, no muy saturados y con alta luminosidad.


### 6.2 Apartado visual

Siguiendo en todo momento la línea estética general del juego, se diferencian diversos elementos del apartado visual:

- **Escenario**.

El escenario es un hexaedro regular o cubo, limitado por seis caras del mismo tamaño y generado a partir de unidades definidas en formato Voxel. Por la superficie del poliedro están repartidas zonas elevadas, zonas llanas y caminos, cada uno diferenciado por un color específico. Por último, una de las caras contiene la charca, es decir, el elemento a defender.

![](https://lh5.googleusercontent.com/1pp7M7gs_g1WoIg_xEuAlbCaI-q7GpncsJo5DxNHqUQL1ByV5s9OGFHVqFiTTYkGQAjDMign6c0eZUf87mPH5SDK2_bB-pbNMn-9uj5dNo75ePGSPz11JoL5_9HwieehbhHEpByZ)

Así pues, en una primera instancia, se limitan los colores del escenario jugable a cuatro.

Se planea incluir diversos biomas u escenarios jugables, cada uno de ellos basado en una paleta de colores diferenciada;

Bioma Pantano:

![](https://lh4.googleusercontent.com/TXgQWhpVlt83etDjwyYZxmVOVPrlITub35fW6DAcFtA7LPzNhg3M7nMgYrNafSEyJ20paOBGp5wyTFgmkCkk-GYfFl1u3ooGeAzqImXpHy3aheO0lrHU4wik3ZZYzqpsGvj-WL_Z)

![](https://lh5.googleusercontent.com/rK1Dd5JWwy1GxaCoPw6uzcGak2qy_9cbq-xHiRPymrxKhMh4TFVeFv0a6Zl4Uu91OFZW7Kf2yeLaik4JcB2CMBWRmC97kNLvhFVIq-XcEEPfvwAeC9l9Nj45nsOXgikCjeNt13u7)


Bioma Ciudad:

![](https://lh6.googleusercontent.com/Z41MUXJ7gsGdB0-wqKRionSkHEM7b0xHw2xt5lHWmSbZVd8jw5R1l-QMfFdtzLF-Hmd7l831v-c1z9vijDtHBnatru3Xj26GpkPvsK-eWeHLULY_-Ki46UXsw2BF9tqVgKnxPi4j)

![](https://lh6.googleusercontent.com/ASA0QnjXO-dbM1D6dp4Rvd5GMp41v-_T6zwML90An7MpX2yYpfsXZUPi5RbpV2A0RamsDm2j-jmorlBmVXc9D14YAohFmSMxr7kL_hMfD1FGYAFQm1TlfeZ-4VzBPz0aG9ZEjoZT)


Bioma Montaña:

![](https://lh4.googleusercontent.com/BK7V2ioEAjoyQnGm7btwQTIIZejfD8ZpiHLmawmiaaD-DrBs647Yx6qjIlPKCiOrht3NV9W28lh3p_V5Z28jlGwWc9kJnl_4ULSFMy1FiRs9qizqyaVCPhjqrk78uTqrp3hNrKuE)

![](https://lh5.googleusercontent.com/AZ-85cmuXKm2jMr9DdmTxCduSSe7394JMhqrcEQBTV-8728i2_fs2FbrR8CIKmWwq_K4elSYTttiHMw3PbmvKVQ9ZpDRKNnWulx2wJtAUYUVHIer9HSS2pcDGWK6wAiTRKc8pArr)

Otros biomas:

![](https://lh5.googleusercontent.com/pCXfBnfKCp12tBGAqrz_iixI9uXFEhYbyeM49xcxCWKXDhxbzPcPPdqNF7SUs7KB145WxYBPg7mwlvkVP6AptwDWy7o2m8xY82EaDP3gtb4DAlJru1ZGoRPpUliR0CEQlaWqv1OD)

![](https://lh3.googleusercontent.com/sZM54ldy0WYvTPkmVwHaDWvQA5AxngABtHF8KJui1eTovgTu9EuEFM6PweAag9n3FWCKl7JXVbOHLXRHcAuTjNh3F2RRyddz6-hI7ra0Aa8PuNhc_57iba3Apb0qG1kBLFKADIUJ)

- **Personajes**

La representación en el escenario tanto de las defensas como de los enemigos son modelos Voxel de dimensiones 16x16x16 voxeles.

- Torre básica:

![](https://lh5.googleusercontent.com/XNT-bL2hfcgTNgpkOQsYFB5iP4YnurFkLsm7gyMbDb4IRbBP2g8J4YSuQupDQS8a_RXvc7q3q0-2JE_9D7VLZQGJyOSh7ZMQV6bNyDTu0dnPl2kqX39gIiY5j7NxJEIa6T9gV-5U)

- Torre pesada:
- Torre psíquica:
- Torre montañosa:
- Enemigos:

A su vez, la representación mediante cartas o pantallas de información es en gráficos 2D acorde a las descripciones de la estética general y al resto de elementos 2D.

Ejemplo, torreta pesada:

![](https://lh4.googleusercontent.com/Dzd7nWF57Rs77VOYupw-UyEDxH8ne5-6BegqIbMsGoOlOjXHHTOmph_U8cBsECglTBBoB5a21ZOJfKTo5pBb2WBmR24A_NquxksyRXWxeH6uMW1kYXeZau0la7q7CbZvrNM9ieAf)


### 6.3 Música


### 6.4 Ambiente sonoro



## 


## 7. Interfaz


### 7.1 Diseños básicos de los menús

Para la interfaz se cuentan con 7 ventanas:

- Menú principal:

![Recurso 1](https://lh6.googleusercontent.com/rCV4rqVBA8GQbaqIftoS6yrzmLV9DwA7l6kJW7XkonW1ULULik42KtX_4j6akqnKth_PsSnr87lHH7DmWR3j8Hmg9wh0v4ivM9CVmDRlU_JMkhv9aXNCumx1oYIlLnnbNdzS2AiH)

- Ajustes:

![Recurso 4](https://user-images.githubusercontent.com/56488179/138528974-991d5275-41b5-4bf1-a620-c50ca0b1f0b7.png)

- Tutoriales:

![Recurso 8](https://user-images.githubusercontent.com/56488179/138528983-708fb63e-830f-4603-b29a-61109a15d99c.png)

- Selector de nivel:

![Recurso 9](https://user-images.githubusercontent.com/56488179/138529005-17654b79-0642-410d-97b6-890ab7c430d7.png)

- Menú de pausa:

![Recurso 5](https://user-images.githubusercontent.com/56488179/138529015-26a65fcd-6918-43c8-b392-2fea20f42fd0.png)

- Final de nivel:
- 
![Recurso 6](https://user-images.githubusercontent.com/56488179/138529024-42a8e7bd-ff84-42a7-b9b5-fdfce59aea37.png)

- Interfaz de juego:

![Recurso 7](https://user-images.githubusercontent.com/56488179/138528992-09385b3c-c4b4-493f-b28d-f235ab558277.png)

### 7.2 Diagrama de flujo

Flujo de escenas:

![](https://lh6.googleusercontent.com/7k6M4w1ZdGuKuBK72xem0Uu5PHNoBZPJjpaYxPJgbp3hJ1ydfIefFEy0RCkq3md5ElrEPwxL41SNHyZbHJ82DnDf-CXiI_CyJ198rYmyd8uAWpHVXEuaZW8tnHJZfTWVvFW_Q90J=s1600)

Flujo de juego:

![](https://lh3.googleusercontent.com/aF1nWLFmWOgguDnGZoJvcOlg-Rg5idqPTJ36BbybrI7znmQiQz3duVFWYjOZu0G9ywJHTiWaXSx0U-r1mU3JT1imTOPqgxp3-cy9zKN_-aYHIY8eDtvKcv5eABgkfKF-SN-zxnpM=s1600)

## 


## 8. Hoja de ruta del desarrollo


### 8.1 Hito 1: Alfa

- Motor Voxel que genere de manera procedural el cubo, los caminos y los obstáculos: 26/10/2021
- Funcionalidades básicas de juego (IA, vida y ataque):25/10/2021
- Juego responsive: 18/10/2021.
- Torres y enemigos definidos, con concepts y algún modelo de ellos: 29/10/2021
- Campaña de marketing iniciada (redes sociales preparadas): 11/10/2021


### 8.2 Hito 2: Beta

- Interfaz implementada con estructura final y arte de GUI final:
- Tutorial introductorio en el que se expliquen las mecánicas básicas:
- Balanceo de las estadisticas de enemigos y torres para hacer el juego divertido y justo:
- Agregar estanque desde el que spawnean los enemigos:
- Ajustar la generación de caminos:
- Crear y meter en el juego los 6 modelos de enemigo y los 5 modelos de torre/defensa:
- Nueva mecánica característica
- Añadir offset para la colocación de torres:

### 8.3 Hito 3: Gold


### 8.4 Fecha de lanzamiento


  
