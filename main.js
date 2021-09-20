window.onload = ()=>{
    document.getElementById("logo").addEventListener("click",scrollTop);
    document.getElementById("title").addEventListener("click",scrollTop);
    window.addEventListener("scroll", scrollListener);
}

function scrollTop(){
    window.scrollTo(0,0);
}

function scrollListener(e){
    let nav = document.querySelector("nav");
    let parallaxFronts = document.querySelectorAll(".parallax-front");
    if(window.scrollY > 1){
        nav.classList.remove("hidden");
    }
    else{
        nav.classList.add("hidden");
    }
    for(let i = 0; i < parallaxFronts.length; i++){
        let topside = parallaxFronts[i].getBoundingClientRect().top;
        console.log(topside);
        if(topside >= 0 && topside <= window.innerHeight){
            //let ratio = (Math.cos(topside*Math.PI/window.innerHeight+Math.PI/2)+1)*12.5;
            //let ratio = topside/window.innerHeight *25;
            let ratio = (topside/window.innerHeight - 0.2) *33;
            parallaxFronts[i].style.transform = "translateY(" + ratio +"%)"
        }
    }
}

