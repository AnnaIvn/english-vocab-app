let words = [];
let currentIndex = 0;
let isFlipped = false;

function flipCard() {
    let card = document.getElementById("word-card");
    card.classList.toggle("flipped");
    if (isFlipped) {
        card.querySelector(".front").textContent = words[currentIndex].Name;
    } else {
        card.querySelector(".front").textContent = words[currentIndex].Meaning;
    }
    isFlipped = !isFlipped;
}

function updateCard() {
    isFlipped = false;
    let card = document.getElementById("word-card");
    card.classList.remove("flipped");
    card.querySelector(".front").textContent = words[currentIndex].Name;
    card.querySelector(".back").textContent = words[currentIndex].Meaning;
    document.getElementById("prev-btn").disabled = currentIndex === 0;
    document.getElementById("next-btn").disabled = currentIndex === words.length - 1;

    // Update the word counter
    document.getElementById("word-counter").textContent = (currentIndex + 1) + " / " + words.length;
}

function prevWord() {
    if (currentIndex > 0) {
        currentIndex--;
        updateCard();
    }
}

function nextWord() {
    if (currentIndex < words.length - 1) {
        currentIndex++;
        updateCard();
    }
}

function initializeWords(jsonData) {
    words = jsonData;
    updateCard();
}

window.flipCard = flipCard;
window.prevWord = prevWord;
window.nextWord = nextWord;
window.initializeWords = initializeWords;
