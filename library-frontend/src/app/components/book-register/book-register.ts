import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, signal, ViewChild, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';

@Component({
  selector: 'app-book-register',
  imports: [TranslateModule, NgxMaskDirective, ReactiveFormsModule, CommonModule],
  providers: [provideNgxMask()],
  templateUrl: './book-register.html',
  styleUrl: './book-register.css'
})
export class BookRegister implements OnInit {
  bookForm!: FormGroup;
  fileName: WritableSignal<string | null> = signal(null);
  imagePreview: WritableSignal<string | ArrayBuffer | null> = signal(null);

  @ViewChild('fileInput') fileInputRef!: ElementRef<HTMLInputElement>;
  selectedFile: File | null = null; // Continua sendo uma propriedade regular, pois não é diretamente renderizada no template

  constructor(private fb: FormBuilder, private translate: TranslateService) { }

  ngOnInit(): void {
    this.bookForm = this.fb.group({
      title: ['', Validators.required],
      isbn: ['', [Validators.required, Validators.pattern(/^(97(8|9))?\d{9}(\d|X)$/)]],
      author: ['', Validators.required],
      genre: [''],
      publisher: ['', Validators.required],
      year: ['', [Validators.required, Validators.pattern(/^\d{4}$/)]],
      synopsis: ['', [Validators.required, Validators.maxLength(5000)]],
      coverImage: [null, Validators.required]
    });
  }

  onSubmit() {
    if (this.bookForm.valid) {
      const formData = new FormData();
      Object.keys(this.bookForm.value).forEach(key => {
        formData.append(key, this.bookForm.value[key]);
      });

      // Here you would typically send the formData to your backend service
      console.log('Form submitted successfully', formData);
    } else {
      console.log('Form is invalid');
    }
  }

  onClearFields() {
    this.bookForm.reset();
    this.resetFileSelection();
    this.bookForm.updateValueAndValidity();
  }

  onFileSelected(event: Event): void {
    const element = event.target as HTMLInputElement;
    const file = element.files?.[0];

    if (!file) {
      this.resetFileSelection();
      return;
    }

    if (!file.type.startsWith('image/')) {
      this.resetFileSelection();
      
      this.translate.get('REGISTER.IMAGE_ERROR_MESSAGE').subscribe((message: string) => {
        alert(message);
      });

      if (this.fileInputRef) {
        this.fileInputRef.nativeElement.value = '';
      }
      return;
    }

    this.fileName.set(file.name);
    this.selectedFile = file;
    this.bookForm.get('coverImage')?.setValue(file);

    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview.set(reader.result);
    };
    reader.readAsDataURL(file);
  }

  private resetFileSelection(): void {
    this.fileName.set(null);
    this.imagePreview.set(null);
    this.selectedFile = null;
    this.bookForm.get('coverImage')?.setValue(null);
  }
}
