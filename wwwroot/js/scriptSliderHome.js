$('.owl-carousel').owlCarousel({
    loop: true,
    margin: 0,
    responsiveClass: true,
    responsive: {
        0:{
            items: 2,
        },
        768:{
            items: 4,
        },
        1100:{
            items: 4,
        },
        1400:{
            items: 4,
            loop: false,
        }
    }
});