const board = document.getElementById('gameBoard');
const scoreLabel = document.getElementById('score');
const messageLabel = document.getElementById('message');
const restartButton = document.getElementById('restartButton');

// Adjust this value to change the overall snake speed.
const GAME_TICK_MS = 160;
const gridSize = Number(board.dataset.gridSize || 20);
let snake = [];
let food = { x: 0, y: 0 };
let direction = 'RIGHT';
let nextDirection = 'RIGHT';
let gameOver = false;
let score = 3;
let intervalId = null;

function createBoard() {
  board.innerHTML = '';
  for (let index = 0; index < gridSize * gridSize; index += 1) {
    const cell = document.createElement('div');
    cell.className = 'cell';
    board.appendChild(cell);
  }
}

function initializeGame() {
  createBoard();
  snake = [
    { x: 10, y: 10 },
    { x: 9, y: 10 },
    { x: 8, y: 10 }
  ];
  direction = 'RIGHT';
  nextDirection = 'RIGHT';
  gameOver = false;
  score = 3;
  messageLabel.textContent = 'Press any arrow key to start.';
  scoreLabel.textContent = `Score: ${score}`;
  spawnFood();
  draw();
  if (intervalId) {
    clearInterval(intervalId);
  }
  intervalId = setInterval(moveSnake, GAME_TICK_MS);
}

function spawnFood() {
  let newFood = { x: 0, y: 0 };
  do {
    newFood = {
      x: Math.floor(Math.random() * gridSize),
      y: Math.floor(Math.random() * gridSize)
    };
  } while (snake.some(segment => segment.x === newFood.x && segment.y === newFood.y));

  food = newFood;
}

function draw() {
  const cells = Array.from(board.children);
  cells.forEach(cell => cell.className = 'cell');

  snake.forEach(segment => {
    const index = segment.y * gridSize + segment.x;
    const cell = cells[index];
    if (cell) {
      cell.classList.add('snake');
    }
  });

  const foodIndex = food.y * gridSize + food.x;
  const foodCell = cells[foodIndex];
  if (foodCell) {
    foodCell.classList.add('food');
  }
}

function updateDirection(newDirection) {
  const oppositeDirections = {
    UP: 'DOWN',
    DOWN: 'UP',
    LEFT: 'RIGHT',
    RIGHT: 'LEFT'
  };

  if (oppositeDirections[direction] === newDirection) {
    return;
  }

  nextDirection = newDirection;
}

function moveSnake() {
  if (gameOver) {
    return;
  }

  direction = nextDirection;
  const head = { ...snake[0] };

  switch (direction) {
    case 'UP':
      head.y -= 1;
      break;
    case 'DOWN':
      head.y += 1;
      break;
    case 'LEFT':
      head.x -= 1;
      break;
    case 'RIGHT':
      head.x += 1;
      break;
  }

  if (isWallCollision(head) || isSelfCollision(head)) {
    gameOver = true;
    messageLabel.textContent = 'Game Over! Press restart to play again.';
    clearInterval(intervalId);
    return;
  }

  snake.unshift(head);

  if (head.x === food.x && head.y === food.y) {
    score += 1;
    scoreLabel.textContent = `Score: ${score}`;
    spawnFood();
  } else {
    snake.pop();
  }

  draw();
}

function isWallCollision(position) {
  return position.x < 0 || position.y < 0 || position.x >= gridSize || position.y >= gridSize;
}

function isSelfCollision(position) {
  return snake.slice(1).some(segment => segment.x === position.x && segment.y === position.y);
}

window.addEventListener('keydown', event => {
  const keyMap = {
    ArrowUp: 'UP',
    ArrowDown: 'DOWN',
    ArrowLeft: 'LEFT',
    ArrowRight: 'RIGHT'
  };

  if (keyMap[event.key]) {
    event.preventDefault();
    updateDirection(keyMap[event.key]);
  }
});

restartButton.addEventListener('click', initializeGame);

initializeGame();
