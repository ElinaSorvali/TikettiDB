describe('TikettiDB', function () {
    beforeEach(() => {
        //haluttu sivu
        cy.visit('https://localhost:44347/IT_tukihenkilot/')
    })

    it('Lisätään uusi it-tukihenkilö', function () {
        //Klikataan "Add new" -nappia, joka ei ole nappi, niin siksi tuo a#
        cy.get('a#ithenkilotCreate').click()

        //Täytetään kentät
        cy.get('input[name="Etunimi"]').type('Tiina')
        cy.get('input[name="Sukunimi"]').type('CypressTestaaja')
        cy.get('input[name="Puhelinnro"]').type('123')
        cy.get('input[name="Sahkoposti"]').type('tiina@testaa.fi')
        cy.get('input[name="Salasana"]').type('999')
        cy.get('input[name="ConfirmPassword"]').type('999')
        cy.get('input[type="radio"][value="2"]').check()
        //Tallennetaan tiedot
        cy.get('button.btn-success').click()
    })

    it('muokataan tiina pääkäyttäjäksi', function () {
        cy.wait(1000) //Lisätään viive että taulukko päivittyy
        //tarkempi haku löytää Tiina CypressTestaajan
        cy.get('table.table-hover tbody')
            .find('tr')
            .should('contain', 'Tiina')
            .and('contain', 'CypressTestaaja')

        //Klikataan editnappia Tiinan riviltä
        cy.get('table.table-hover tbody')
            .contains('tr', 'Tiina CypressTestaaja')
            .within(() => {
                cy.get('button.edititHenkiloID').click() //Klikataan edit-nappia tässä rivissä
            })

        // Valitse dropdownista ykkönen
        cy.get('select[name="Taso"]').select('1') 
        //Tallennetaan tiedot
        cy.get('button.btn-success').click()

    })
    it('Poistetaan äsken tehty tiina', function () {
        cy.wait(1000) //Lisätään viive että taulukko päivittyy
        //tarkempi haku löytää Tiina CypressTestaajan
        cy.get('table.table-hover tbody')
            .find('tr')
            .should('contain', 'Tiina')
            .and('contain', 'CypressTestaaja')

        //Klikataan poistonappia Tiinan riviltä
        cy.get('table.table-hover tbody')
            .contains('tr', 'Tiina CypressTestaaja')
            .within(() => {
                cy.get('button.haeitHenkiloID').click() //Klikataan delete-nappia tässä rivissä
            })

        //Klikataan vahvistusnappia poistaaksesi henkilön
        cy.get('#ModalITDelete').within(() => {
            cy.get('button.btn-danger').click()
        })

    })
})