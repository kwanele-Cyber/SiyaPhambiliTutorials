// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//JavaScript (for tab switching functionality)
document.querySelectorAll('.tab-button').forEach(button => {
    button.addEventListener('click', () => {
        const activeButton = document.querySelector('.tab-button.active');
        activeButton.classList.remove('active');
        
        const activeContent = document.querySelector('.tab-content.active');
        activeContent.classList.remove('active');
        
        button.classList.add('active');
        const index = Array.from(document.querySelectorAll('.tab-button')).indexOf(button);
        const newActiveContent = document.querySelectorAll('.tab-content')[index];
        newActiveContent.classList.add('active');
    });
});
