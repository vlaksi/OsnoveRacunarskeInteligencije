





<h1 align = "center"> Markovljevi procesi odlucivanja </h1>

<p align="center">

  <img width="1000" height="400" src="https://user-images.githubusercontent.com/45834270/77256491-fde3f980-6c6e-11ea-8e48-33e93b427865.png">

</p>

## Markovljevo
**Markovljevo** znaci da ako posmatramo trenutno stanje, proslost i buducnost su nezavisni(ishod akcije koju sad radimo ne zavisisi od toga koje smo akcije radili pre nje)

## Politike
Kod deterministickih pretraga trazili smo ***plan*** tj. niz akcija koje nas vode od starta do cilja. A kod **MDPs** nam je potrebna optimalna **politika** 
<p align="center">

  <img width="170" height="75" src="https://user-images.githubusercontent.com/45834270/77257031-173a7500-6c72-11ea-9c3b-22de8d81c93c.png">

</p>

  - politika za *svako stanje* kaze koju akciju treba odigramo
  - ***optimalna politika*** je ona koja maksimizuje ocekivanu korist

## Zanemarivanje
<p align="center">

  <img width="1000" height="400" src="https://user-images.githubusercontent.com/45834270/77258939-3212e680-6c7e-11ea-8d01-cd8e8d0b826a.png">

</p>
<p align="center">

  <img width="280" height="500" src="https://user-images.githubusercontent.com/45834270/77259225-5a9be000-6c80-11ea-8cb7-9d279396da1e.png">

</p>

## Rezime definicije MDP

<p align="center">

  <img width="800" height="300" src="https://user-images.githubusercontent.com/45834270/77259560-90da5f00-6c82-11ea-9d4b-31141c655ff1.png">

</p>


<p align="center">

  <img width="800" height="376" src="https://user-images.githubusercontent.com/45834270/77259807-5e316600-6c84-11ea-9dd7-d88f0fb28c22.png">

</p>

## Bellman updates
<p align="center">

  <img width="800" height="256" src="https://user-images.githubusercontent.com/45834270/77260669-ccc4f280-6c89-11ea-9a27-5bcd100b2229.png">

</p>

<p align="center">

  <img width="800" height="256" src="https://user-images.githubusercontent.com/45834270/77260856-c2572880-6c8a-11ea-82be-4736ef100337.png">

</p>

<p align="center">

  <img width="800" height="406" src="https://user-images.githubusercontent.com/45834270/77260900-05b19700-6c8b-11ea-8ad3-721bb6229c99.png">

</p>

<p align="center">

  <img width="800" height="406" src="https://user-images.githubusercontent.com/45834270/77261747-0f3dfd80-6c91-11ea-8056-cad5dc784265.png">

</p>

## Napomena
Svi primeri ilustrovani su primena znanja koja se zahteva na ispitu.




## Odredjivanje akcija iz vrednosti
Ako predpostavimo da imamo *optimalne vrednosti*, nije bas ocigledno koje bi akcije trebali izvrsiti, stoga radimo *jedan korak* ***mini-expectimax-a***

<p align="center">

  <img width="500" height="75" src="https://user-images.githubusercontent.com/45834270/77262036-41e8f580-6c93-11ea-9bba-51382663848d.png">

</p>

  - ovo se naziva ***ekstrakcijom politike*** jer nam daje politiku koja je implicitno data *vrednostima*
  - argmax vraca akciju


<p align="center">

  <img width="800" height="406" src="https://user-images.githubusercontent.com/45834270/77262361-92615280-6c95-11ea-860d-26021576415e.png">

</p>

<p align="center">

  <img width="800" height="406" src="https://user-images.githubusercontent.com/45834270/77264838-85dff880-6c9b-11ea-985c-e7e0b37da056.png">

</p>

<p align="center">

  <img width="800" height="406" src="https://user-images.githubusercontent.com/45834270/77262106-c63b7880-6c93-11ea-8e09-58c913e7ecd1.png">

</p>

<p align="center">

  <img width="800" height="353" src="https://user-images.githubusercontent.com/45834270/77265771-e5d79e80-6c9d-11ea-82ae-5f5984e06be4.png">

</p>

<p align="center">

  <img width="800" height="353" src="https://user-images.githubusercontent.com/45834270/77266030-a2c9fb00-6c9e-11ea-88b4-fa395d8284d7.png">

</p>

<p align="center">

  <img width="800" height="353" src="https://user-images.githubusercontent.com/45834270/77266177-03f1ce80-6c9f-11ea-8ac5-330a6e2a0ef8.png">

</p>

