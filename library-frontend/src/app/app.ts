import { Component, OnInit } from '@angular/core';
import { RouterOutlet, RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterModule, TranslateModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App implements OnInit {
  protected title = 'library-frontend';

  constructor(public translate: TranslateService) { }

  ngOnInit() {
    this.translate.addLangs(['en-us', 'pt-br']);
    this.translate.setDefaultLang('pt-br');

    const storedLang = localStorage.getItem('language');

    if (storedLang && this.translate.langs.includes(storedLang)) {
      this.translate.use(storedLang);
    } else {
      const browserLang = this.translate.getBrowserLang();
      const validBrowserLang = browserLang?.match(/en-us|pt-br/) ? browserLang : 'pt-br';
      this.translate.use(validBrowserLang);
      localStorage.setItem('language', validBrowserLang);
    }
  }

  switchLanguage(language: string) {
    this.translate.use(language);
    localStorage.setItem('language', language);
  }

  getFlagIcon(language: string): string {
    switch (language) {
      case 'pt-br':
        return 'https://flagcdn.com/16x12/br.png';
      case 'en-us':
        return 'https://flagcdn.com/16x12/gb.png';
      default:
        return '';
    }
  }
}