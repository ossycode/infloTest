/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./**/*.{razor,html,cshtml}"
    ],
    theme: {
        extend: {
            colors: {
                grayClr: "#7a7a7a",
                yellowClr: "rgba(251,176,56,1.0)",
                offYellowClr: "rgba(160,178,68,1.0)",
                yelloGradient: "linear-gradient(left,#fbb038 0%,#fbb038 25%,#a0b244 50%,#39b54a 75%,#30955c 100%)",
                offOrange: "#f0852e"
            }
        },
    },
    plugins: [
    ],
}
