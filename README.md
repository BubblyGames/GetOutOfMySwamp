

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

Es un tower defense 3D con vista isométrica en un mapa cúbico. El origen de los enemigos y la meta están en caras opuestas y tienen que recorrer una serie de caminos pasando por el resto de caras para llegar al final. Los caminos serán semi procedimentales, así que el jugador tendrá en primera instancia que familiarizarse con el mapa que le tocó para defenderlo adecuadamente. Los enemigos aparecen por oleadas con una dificultad que aumenta progresivamente, tanto por un aumento de número como por diversidad de tipos de enemigos. Defender satisfactoriamente tiene una recompensa económica para poder seguir expandiendo las defensas y mejorar las ya existentes. Todas las torres aparecen disponibles desde un inicio y el único factor limitante será su precio. Si una rana enemiga consigue llegar al final se reduce una cantidad determinada de vida hasta que llegue a 0 y se acabe la partida.


### 4.2 Descripción detallada de las mecánicas de juego

![](https://lh4.googleusercontent.com/7_tib0ycPSkRPNOmPVR7lpDnccMY1CH9RyNUzCjInd67h-6ZX0qLFfUIJddRScMD8ORfV0BSEqDi8Iqpj8Wfii9Jr68iM_2h8t9bEcG8d3XK19vl0DOSLOA-FJglhf44LmBII0In=s1600)

#### 4.2.1 Victoria y derrota

Para ganar la partida se ha de defender un número determinado de oleadas y con la vida igual o superior a uno. El número de oleadas defendidas y por defender estará en pantalla al inicio de cada ronda. Si llegan a la meta un número de enemigos igual a la cantidad de vidas la partida se acaba.


#### 4.2.2 Mapa

El mapa tiene forma de cubo, con sus seis caras siendo terreno jugable. Las caras visibles serán de una a tres, dependiendo de la rotación del cubo que elija el jugador. El cubo se puede rotar en cualquier momento de la partida menos cuando se coloca una torre.

En el mapa hay tres tipos de casillas: Casillas de camino, por las que los enemigos transitan y se pueden poner trampas; Casillas de terreno construible, donde se podrá poner cualquier tipo de torre; y casillas obstáculo, en las que no puede haber camino ni estructuras.


#### 4.2.3 Economía

El juego comienza con una cantidad determinada de monedas para poner las torres iniciales. Una vez iniciada la partida, la fuente de ingresos será la derrota de los enemigos y el edificio que genera monedas. El dinero conseguido se puede gastar en construir más estructuras o mejorar las ya existentes.


#### 4.2.4 Defensas y enemigos

Existen dos tipos de defensas: Las torres y las trampas. Las torres tienen alcance, daño, velocidad de ataque y éstos pueden ser individuales o en área o tener efectos de estado sobre los enemigos. Se puede construir en cualquier lugar menos caminos y rocas, a excepción de la torre económica que solo se puede poner adyacente al camino. Además, tienen tres niveles de mejoras para aumentar sus estadísticas. Luego están las trampas, las cuales se colocan en el camino del enemigo, tienen daño, durabilidad y pueden tener efecto.

Por otro lado están los enemigos, con características de vida y velocidad de movimiento. Aparecen por oleadas y aumentan en número conforme avanzas en la partida. Tienen diferentes roles en función de sus estadísticas, siendo los roles unidad básica, tanque y horda, explicadas más adelante en el apartado Enemigos.


![](https://lh6.googleusercontent.com/XyZfstCSw-DLWgamwydXli8vhxtgT-UwKJedln-d8n8b128AX2uwMAu_ZfEi14CkXv6f0j37Y27lFRzzMlJHlza6lMVwx9gM2rYwonhWBWuMDhGatPP_NvB05I83jCBoDTnug5R2=s1600)




### 4.3 Controles

El jugador durante la partida tendrá dos tareas principales, vigilar el mapa para saber el estado de la partida y colocar defensas. Para vigilar el mapa podrá rotarlo con click (pulsar en el caso de móvil) y arrastrar. Luego para poner las torres será exactamente el mismo input, click y arrastrar desde la tarjeta de la torre hasta la posición en la que desee ponerla.


### 4.4 Niveles y misiones

El juego cuenta con cinco mapas, diferenciados a nivel visual y en la generación del terreno. El objetivo en todos ellos es defender tu estanque de las tropas enemigas.

Los niveles son semi procedimentales y se crean mediante una semilla.


### 4.5 Torres y trampas

| **TORRES**     | ALCANCE      | DAÑO           | OBJETIVO          | VELOCIDAD DE ATAQUE | EFECTO                            | COSTE  |
| -------------- | ---------    | -------------- | ----------------- | ------------------- | --------------------------------- |--------|
| TORRE PESADA   | Largo (7 ud) | Elevado (35 ud)| ÁreaTT\*          | Lento (0.1 ud)      | \-                                | 300    |
| TORRE BÁSICA   | Medio (5 ud) | Medio (10 ud)  | IndividualTA-TT\* | Medio (1 ud)        | \-                                | 125    |
| TORRE PSÍQUICA | Corto (3 ud) | Bajo (7 ud)    | IndividualTT\*    | Medio (1 ud)        | El enemigo se ralentiza           | 200    |
| TORRE ECONÓMICA| Nada         | Ninguno        | Ninguno           | Sin ataque          | Genera ingresos de forma pasiva   | 400    |
| **TRAMPAS**    |              |                |                   |                     |                                   |        |
| TRAMPA PANTANO | 1 Casilla    | \-             | ÁreaTT\*          | \-                  | Ralentiza                         | X      |

Cada defensa del jugador se puede mejorar, por separado, hasta tres veces, aumentando con estas mejoras las estadísticas de dicha defensa y haciéndola más eficaz en combate.

Nivel 1:
| **TORRES**     | ALCANCE      | DAÑO           | OBJETIVO          | VELOCIDAD DE ATAQUE | EFECTO                            | COSTE  |
| -------------- | ---------    | -------------- | ----------------- | ------------------- | --------------------------------- |--------|
| TORRE PESADA   | Largo (7 ud) | Elevado (70 ud)| ÁreaTT\*          | Lento (0.1 ud)      | \-                                | 150    |
| TORRE BÁSICA   | Medio (5 ud) | Medio (12 ud)  | IndividualTA-TT\* | Medio (1.5 ud)      | \-                                | 50     |
| TORRE PSÍQUICA | Corto (4 ud) | Bajo (8 ud)    | IndividualTT\*    | Medio (1 ud)        | El enemigo se ralentiza           | 100    |

Nivel 2:
| **TORRES**     | ALCANCE      | DAÑO           | OBJETIVO          | VELOCIDAD DE ATAQUE | EFECTO                            | COSTE  |
| -------------- | ---------    | -------------- | ----------------- | ------------------- | --------------------------------- |--------|
| TORRE PESADA   | Largo (12 ud)| Elevado (70 ud)| ÁreaTT\*          | Lento (0.1 ud)      | \-                                | 200    |
| TORRE BÁSICA   | Largo (8 ud) | Medio (14 ud)  | IndividualTA-TT\* | Medio (1.5 ud)      | \-                                | 100    |
| TORRE PSÍQUICA | Medio (5 ud) | Medio (9 ud)   | IndividualTT\*    | Medio (1 ud)        | El enemigo se ralentiza           | 150    |

Nivel 3:
| **TORRES**     | ALCANCE      | DAÑO           | OBJETIVO          | VELOCIDAD DE ATAQUE | EFECTO                            | COSTE  |
| -------------- | ---------    | -------------- | ----------------- | ------------------- | --------------------------------- |--------|
| TORRE PESADA   | Largo (12 ud)| Elevado (70 ud)| ÁreaTT\*          | Lento (0.2 ud)      | \-                                | 300    |
| TORRE BÁSICA   | Largo (10 ud)| Medio (14 ud)  | IndividualTA-TT\* | Rápido (1.75 ud)    | \-                                | 150    |
| TORRE PSÍQUICA | Medio (5 ud) | Medio (9 ud)   | IndividualTT\*    | Medio (1.5 ud)      | El enemigo se ralentiza           | 200    |

\*TT: Ataques desde Tierra a objetivos Terrestres.
TA: Ataques desde Tierra a objetivos Aéreos.

### 4.6 Enemigos

| **ENEMIGOS**                | VIDA | VELOCIDAD DE MOVIMIENTO | TIPO                 |
| --------------------------- | ---- | ----------------------- | -------------------- |
| NORMAL                      |  40  | Media                   | Terrestre - Normal   |
| TANQUE                      | 250  | Lenta                   | Terrestre - Pesada   |
| HORDA                       |  25  | Media                   | Terrestre - Horda    | 

### 4.7 Evento aleatorio

**Proyectil:** Aleatoriamente se producirán explosiones en el mapa generando cráteres, destruyendo torres y enemigos y modificando la geometría del cubo, junto con el camino si impacta contra él. 

## 5. Trasfondo


### 5.1 Descripción detallada de la historia y la trama

Dos sociedades de ranitas viven cada una en un estanque de un cubo espacial en paz y armonía. Pero una de las sociedades ha ensuciado su estanque hasta tal punto de volverlo tóxico e inhabitable, lanzando una invasión al estanque de las ranas limpias para poder encontrar un nuevo lugar donde continuar sus actividades.

Las ranas limpias, ante esta situación, deben defender su estanque de la invasión de las ranas sucias.


### 5.2 Personajes

Hay dos personajes característicos en el juego: el líder de las ranas limpias y el líder de las ranas sucias.


### 5.3 Entornos y lugares

Hay cinco niveles jugables, cada uno con su estética única: Pantano, Tundra, Otoño, Cerezo y Nevado.

- Primer mapa: Pantano. El estanque de las ranas limpias es un estanque limpio, vívido y con nenúfares mientras que el estanque de las ranas sucias es oscuro, con lodo.
- Segundo mapa: Tundra. Antes de llegar a las frías tierras del ártico, las ranas se refugian en un charquito de aguas termales, a donde acuden grandes ranas cubiertas de fango. 
- Tercer mapa: Otoño. Con la caída de las hojas y las temperaturas, el suelo se tiñe de marrón y nuestras ranitas buscan el calor grupal en un pequeño estanque. 
- Cuarto mapa: Cerezo. Los cerezos anuncian la llegada de la primavera con su lluvia de pétalos rosas, dando al campo un color jovial y acompañando a una plácida mañana soleada.
- Quinto mapa: Nevado. Las huellas quedan marcadas en la nieve ante el paso de nuestras protagonistas, que se dirigen a una calentita charca. 


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

El juego cuenta con una sola canción (autor: [Pascual Gázquez Compán](https://open.spotify.com/artist/7y2dfMVexZgClCIeiHd22f?si=Q2ch-_AVR8CC8a927irtxw&utm_source=copy-link)) la cual se reproduce tanto en los menúes como durante la partida.

### 6.4 Ambiente sonoro

El juego cuenta con efectos de sonido para la construcción de las torres y sus mejoras, para la muerte de las ranas enemigas y para cuando estas llegan a tu estanque, inflingiendote daño, todas cortesía de [Zapsplat](https://www.zapsplat.com/)

El efecto de sonido de la construcción de las torres y de la mejora de estas es la misma. Es el sonido de unos ladrillos infantiles agitándose en una bolsa.
El efecto de sonido de la muerte de las ranas consiste en el croar de una rana.
El efecto de sonido cuando los enemigos llegan a tu estanque varía entre 3 efectos de sonido distintos: 
- La agitación de una loción de bebé en un bote
- 2 chapoteos distintos en el agua

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

## 8. Post-Mortems


## 8.1 Post-Mortem de la etapa Alfa

## Autocríticas individuales

### Gonzalo

Debo de ser más duro. La gente ha llegado y en varias ocasiones han saltado a trabajar en tareas que todavía no tenían asignadas y que eran menos prioritarias para el proyecto que las que ya tenían asignadas en ese momento. También he tenido deslices con la programación de reuniones y el tratamiento de redes como olvidarme de fechas, elementos que debían estar presentes en las publicaciones, publicar en unas redes pero olvidarme de publicar en otras…

Lo bueno es que algunas cosas sí que he logrado corregirlas de distintas formas, como ponerme recordatorios de las reuniones los días previos o mirar el GDD para asegurarme de que lo subía a las redes que teníamos pensadas, con la información como hastags debidamente puestos.

Tengo que mejorar mi atención o mis habilidades de gestión, en general. Aunque creo que parto de buen nivel. Ha sido una constante el comprobar los progresos del resto de miembros del equipo y me he adelantado con el planning de tareas para ver a quién podía encajar en qué tarea cuando terminara su actual, además de contar siempre con responsabilidades de los miembros fuera del proyecto para poder planear siguientes asignaciones de tareas o avances.

### Joy

No considero que sea una persona que destaque en la creatividad así que generalmente suelo estar algo ausente en las tareas iniciales como puede ser el brainstorming para decidir sobre qué irá el juego, aunque luego sí soy bastante crítico si el tema elegido no es de mi agrado, cosa que no me parece justa.

Tras varias vueltas se acabó en una idea que tenía buena pinta. Estuve trabajando en varios prototipos de movimiento que no me llevaron a ningún lado y que debí haber abandonado antes, por suerte otro de mis compañeros del equipo de programación sacó adelante un prototipo sobre el que ya se podía trabajar.

Creo que, en general, he hecho un buen trabajo, pero hay cosas en las que tenía que haberme molestado debido a que somos un estudio pequeño, como conocer el código que hacían mis compañeros en el equipo de programación y comunicar y expresar mejor las ideas diseñadas para que no hubiera desarrollos innecesarios.

### Adrián

Buena comunicación con el resto de los miembros y flujo constante. Trabajo realizado rápido y muy buena gestión del tiempo.

#### A mejorar:

- Seguir mejorando la comunicación e intentar hacerme oír más.
- Seguir mejorando la gestión del tiempo para poder elaborar código más limpio de una vez en vez de tener que ponerlo bonito después de estar funcional.
- Comentar más el código.
- No retrasar las consultas de dudas.

### Dani

Desde el primer momento no me entusiasmó excesivamente la idea de hacer un Tower Defense, pero viendo que el resto de mis compañeros estaban decididos, no me pude negar. Mi mayor preocupación era hacer un juego demasiado simple a nivel técnico, ya que consideraba que, teniendo en cuenta el equipo que teníamos, podíamos aspirar a hacer un juego bastante ambicioso. Al final acepté la idea, pero me propuse hacer un juego que, dentro de un género que no suele llamar la atención, destacara por ser innovador. Al final esto último resultó ser más difícil de lo que parecía.

Terminada ya la Alpha me he dado cuenta de que desarrollar un juego completo y pulirlo a un nivel aceptable, por simple que sea el juego, siempre supone un reto. La generación procedimental del mundo y los caminos, de lo que principalmente me he encargado yo, me ha dado infinitos dolores de cabeza, aunque la satisfacción de verlo todo funcionando al final lo compensa.

### Marta

A nivel personal:

- Ser capaz de expresar abiertamente mi opinión. Una de las cosas que más me ha costado siempre es ser capaz de expresar abiertamente mi opinión. Así que me alegro mucho de haber podido hacerlo en este grupo de trabajo
- Establecer buenos horarios de trabajo. Durante este tiempo de trabajo he sido capaz de establecer unos buenos horarios de trabajo sin llegar a sobreesforzarme. Esto me ha permitido sentirme bien conmigo misma y con el proyecto.
- Problemas con mi pipeline de trabajo. A nivel personal, durante las primeras semanas me demoré más de lo necesario en realizar mis tareas debido a que me intentaba adaptar a lo que el resto del equipo me pedía sin tener en cuenta el flujo de trabajo (a nivel artístico) al que estoy acostumbrada. Esto me llevó a bloquearme en ciertos aspectos, sobre todo referentes al desarrollo conceptual del juego. La solución que propongo es adaptarme a las necesidades de mi equipo, pero sin dejar de tener en cuenta mi propia línea de trabajo de cara a establecer y definir las tareas y/o hitos. Además de esto, me gustaría que las tareas a realizar vinieran de un único canal para evitar problemas de comunicación o errores de prioridad.

### Alberto

Al inicio estaba bastante motivado con el proyecto porque había salido elegido un juego de estrategia, mi género favorito con diferencia, así que quería aplicar mi experiencia en hacer algo interesante. Después no solo había que compartir el rol de game designer sino que al final prácticamente el juego acabó siendo una decisión de todos. Esto, en realidad, a la larga es favorable para el grupo porque trabajamos todos en algo que más o menos nos gusta a todos, pero, siendo yo game designer principalmente, ¿Solo me encargo de escribir cuántas torres y enemigos hay y cómo funcionan? Cosa que también tenía que ser un consenso entre los dos game designers. Aún así creo que podría haber hecho un mayor ejercicio de creatividad para hacer un juego original, ya que lo del cubo no se nos ocurrió a los diseñadores tampoco.

En conclusión, perdí bastante la motivación que tenía al empezar pero no considero que fuese motivo suficiente para no haber dado más de mí en crear alguna mecánica original.

## Críticas de equipo

### Productor

El equipo ha trabajado muy bien. Hemos salido de la Alfa con un juego bastante terminado en lo que a mecánicas se refiere, encima teniendo la procedimentalidad de por medio, algo que complicaba el asunto. A parte de lo mencionado más arriba, un problema que he visto (que va ligado a la cantidad de gente que somos y a circunstancias externas) ha sido el bajo rendimiento del equipo de arte comparado con otros equipos.

Ha habido bastante debate de ideas, algunos que se han alargado hasta otras fases (como es el caso del nombre) o que han ralentizado un poco el desarrollo por alargarse más de lo debido (como fue decidir el tipo de juego y la temática que queríamos abordar). Pero no han sido debates duros, lo cual no ha perjudicado a la sensación de equipo. El equipo, en general, debería de mirar más el GDD, ya que surgen dudas o malentendidos sobre elementos que ya están descritos en el documento.

También destacar que en el equipo de programación han estado brutales: han currado a destajo y han logrado tener muchas mecánicas y funcionalidades listas para la Alfa, algo que baja la intensidad de su trabajo para las siguientes etapas, ya que se centrarán en pulir errores y en expandir o implementar alguna funcionalidad/mecánica extra.

### Programador 1

El trabajo ha sido fluido y sale bien. Hay un buen ambiente de trabajo y se siente el progreso de cada uno del equipo como propio gracias al Trello y las reuniones. Las reuniones semanales son un acierto, aunque posteriormente se debatirán algunos detalles.

Las reuniones deberían contar con un esquema previo y tratar sobre temas específicos en vez de tantas generales para hacerlas más útiles y concisas. También tener fechas de entrega de tareas, siempre teniendo en cuenta la disponibilidad de cada persona, pero tener plazos fijados.

Respecto a la corrección del trabajo de otros miembros mejor que editarlo directamente se cree más conveniente que se le comuniquen las ideas de mejora al encargado de esa tarea y sea él o ella quien realice las modificaciones. Todo esto para ahorrar tiempo y evitar errores inesperados.

### Programador 2

Creo que hemos hecho un buen trabajo. Como equipo siento que en ciertos momentos nos ha faltado un poco de comunicación. Hemos tenido momentos de frustración por no entendernos o por querer hacer las cosas de formas distintas, pero al final el proyecto ha salido adelante más o menos como lo habíamos concebido, así que debemos considerarlo un éxito.

### Programador 3

Considero que todo el equipo ha trabajado correctamente, cada equipo con sus altibajos por diferentes cuestiones:

El equipo de dirección ha trabajado bien encargándose de que todos fuéramos cumpliendo las tareas y organizando una serie de reuniones semanales para ver cómo iban avanzando los equipos.

El equipo de diseño ha hecho un buen trabajo rellenando el GDD con las ideas y mecánicas que decidimos implementar en las reuniones.

El equipo de arte no ha tenido mucho trabajo para la alfa, un par de texturas y modelos han entrado en esta versión. Para compensar el bajo nivel de requerimientos si han trabajado en concepts para avanzar para la siguiente versión.

El equipo de programación ha sido con diferencia el que más trabajo tenía, no ha habido crunch pero sí que ha tenido que dedicarle bastante tiempo tal vez más del debido. Ha sido un buen trabajo en el que cada uno de los integrantes se ha ido dedicando una tarea para avanzar lo más rápido posible, esto, aunque bastante efectivo para implementar la mayoría de las mecánicas si ha provocado de alguno de los integrantes usaran su libre albedrío para implementar las mecánicas como más le apetecía y provocando que otros tuvieran que ir detrás revisando ese trabajo.

#### Arte

A nivel de equipo:

- Correcta asignación de roles. Considero que tenemos un equipo muy compensado y que la asignación de roles ha sido acertada, sobre todo la elección de un productor y Scrum Master, figura que a menudo se ha olvidado o dejado de lado en otros proyectos de ámbito universitario.
- Idea de juego adecuada. La idea de juego quedó muy clara para todos desde un inicio y hemos sabido establecer un buen scope dado el tiempo y los recursos disponibles.
- Buen ambiente y asertividad. El ambiente de trabajo ha sido muy bueno, todo el mundo ha dado su opinión de manera asertiva y eso ha facilitado una correcta comunicación y exposición de las ideas. En este aspecto me he sentido muy cómoda, me alegro de que todos seamos capaces de expresarnos libremente y espero que sigamos haciéndolo.
- Problemas de comunicación y falta de información. Al inicio hubo algunos problemas de comunicación entre las diferentes áreas de trabajo y, si bien pensaba que estaban solucionados, esta última semana de desarrollo se han manifestado nuevas incidencias propiciadas por confusiones evitables o faltas de información. Para solucionar esto, creo que sería necesario comunicar mejor cada una de las tareas realizadas y cada uno de los problemas que nos hemos podido encontrar, así como testear y/o revisar el trabajo de nuestros pares con más frecuencia.
- Presencia en redes sociales. Me he quedado un poco con la sensación de que la presencia en redes sociales la hemos ido improvisando, principalmente por falta de tiempo y material. Creo que sería buena idea elaborar un calendario de contenido para las redes sociales y preparar cada publicación con antelación. Además, de esta forma todos los miembros del equipo estaríamos al tanto de cuáles van a ser las futuras publicaciones y podríamos aportar nuevas ideas, contenido u opiniones.

#### Diseño

En general, en el grupo hay muy buen ambiente de trabajo, consiguen hacer todo estupendamente y a tiempo. En las decisiones grupales nadie discute como si tuviera 12 años y las reuniones son productivas, dentro de lo normal. Considero que soy el menos productivo o el que menos aporta al grupo

### 8.2 Post-Mortem de la etapa Beta

### 8.3 Post-Mortem de la etapa Gold

##

## 9. Hoja de ruta del desarrollo


### 9.1 Hito 1: Alfa

- Motor Voxel que genere de manera procedural el cubo, los caminos y los obstáculos: 26/10/2021
- Funcionalidades básicas de juego (IA, vida y ataque):25/10/2021
- Juego responsive: 18/10/2021.
- Torres y enemigos definidos, con concepts y algún modelo de ellos: 29/10/2021
- Campaña de marketing iniciada (redes sociales preparadas): 11/10/2021


### 9.2 Hito 2: Beta

- Interfaz implementada con estructura final y arte de GUI final:
- Tutorial introductorio en el que se expliquen las mecánicas básicas:
- Balanceo de las estadisticas de enemigos y torres para hacer el juego divertido y justo: 15/11/2021
- Crear y meter en el juego los 6 modelos de enemigo y los 5 modelos de torre/defensa:
- Nueva mecánica característica:
- Añadir offset para la colocación de torres:
- Arreglar proporciones de las torres en el cubo:
- Dirigir correctamente la barra de vida de los enemigos hacia la camara del jugador: 16/11/2021
- Portfolio actualizado en su nueva versión: 

### 9.3 Hito 3: Gold

- Pulir la generación de caminos en el cubo:
- Añadir estanque en el spawn de los enemigos:
- Añadir animaciones a las torres:
- Añadir efectos de partículas al disparo de la torre que ralentiza, a la entrada de los enemigos en el estanque y al caminar de los enemigos:
- Añadir efectos de sonido a la muerte de los enemigos, a la llegada de estos al estanque del jugador y a la construcción y mejora de las torres:

### 9.4 Fecha de lanzamiento

- Campaña de marketing en redes sociales en el que se hable de los distintos elementos del juego:
  
