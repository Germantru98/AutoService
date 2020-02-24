$(function () {
    // Owl Carousel
    var owl = $(".owl-carousel");
    owl.owlCarousel({
        margin: 20,
        loop: true,
        nav: true,
        //items: 8,
        mouseDrag: true,
        dots: false,
        navText: [
            '<span class="arrow-owl arrow-left "><</span>',
            '<span class="arrow-owl arrow-right ">></span>'
        ],
        responsive: {
            0: {
                items: 1
            },
            350: {
                items: 2
            },

            500: {
                items: 3
            },
            650: {
                items: 4
            },
            700: {
                items: 4
            },
            850: {
                items: 5
            },
            1000: {
                items: 6
            },
            1200: {
                items: 7
            },
            1650: {
                items: 8
            }
        }
    });
});