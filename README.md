# OthelloAI

* Projet: ArcOthelloAB
* Auteurs: Arzul Paul & Biloni Kim Aurore
* Date: 17.02.2019
* Organisation: HE-Arc Ingéniere, Neuchâtel
* GitHub: <https://github.com/HE-Arc/ArcOthelloAB>

## Fonction d'évaluation

Le score d'un noeud prend en compte trois paramètres, dans leur ordre d'importance:
1) le nombre de pions possédés par les joueurs
2) la mobilité des joueurs, c'est-à-dire l'évolution des nombres de coups possibles
3) la possession des coins du plateau

Ces valeurs sont ensuite pondérées pour obtenir le score final de l'état de jeu.

### 1. Nombre de pions

Le but du jeu est d'obtenir le plus de pion possible mais la subtilité de l'Othello est qu'il est fréquent de voir la situation ce score s'inverser d'un tour à l'autre.Le nombre de pions ne donne qu'une indication temporaire sur la situation de jeu.

Le score est calculé de la manière suivante:

`Score_NB_pion = NB_pion_joueur - NB_pion_adverssaire`

Ce score est une valeur entière allant, en moyenne, de -10 à 10.

### 2. Mobilité

La mobilité représente le nombre de possibilités d'un joueur. Plus le joueur possède de coups possibles, plus il est mobile. Il est donc important d'utiliser cet aspect pour avoir l'avantage dans le jeu.

Le score de mobilité est calculé de la manière suivante:

`Score_mobilite = NB_coups_possible_actuel - NB_coups_possible_noeud_precedent`

Ce score est une valeur entière allant, en moyenne, de -5 à 5.

### 3. Possession des coins du plateau

Dans le jeu de l'Othello, il est important de posséder les coins du plateau. Ces derniers sont considérés comme stables. Cela signifie qu'une fois qu'un pion est placé  cet endroit, il ne pourra plus être retourné. Il est donc primordial, dans ce jeu, de poser ces pions sur ces cases stables.

Le score de possession des coins est calculé de la manière suivante:

`Score_NB_Coins = NB_coins_possede - NB_de_coins_adverses`

Ce score est une valeur entière allant de -4 à 4.

### Calcul du score final

Le score final est donc un composite des trois scores présentés dans les sections précédentes. Il est cependant important de pondérer ces différents scores. De ce fait, le score final est ainsi donc calculé:

`score = score_NB_pion + score_mobilite * 5 + score_NB_coin * 200`

La pondération choisie vise à privilégier la possession des coins, la mobilité et finalement du nombre de pions obtenus.

### Score d'un état final

Un état final signifie qu'il n'est plus possible de jouer. Deux cas provoquent un état final :

1. le joueur perd
2. le joueur gagne

Si le score final est positif, l'état final est considéré comme gagnant. La valeur du score est donc modifiée et lui est affecté la valeur maximale(`Int32.MaxValue`). Si le score est négatif et donc l'état de jeu considéré comme perdant, la valeur minimale possible lui est affectée(`Int32.MinValue`).

Cela a pour conséquence que notre IA privilégiera toujours les états gagnants.
