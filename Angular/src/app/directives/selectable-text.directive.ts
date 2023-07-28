import { Directive, HostListener, AfterViewInit, ElementRef } from '@angular/core';

@Directive({
  selector: '[selectableText]'
})
export class SelectableTextDirective implements AfterViewInit {
  constructor(private elementRef: ElementRef) { }

  @HostListener('mousedown', [ '$event' ]) onMouseDown($event: Event): void {
    $event.stopPropagation();
  }

  @HostListener('touchstart', [ '$event' ]) onTouchStart($event: Event): void {
    $event.stopPropagation();
  }

  ngAfterViewInit(): void {
    this.elementRef.nativeElement.style['user-select'] = 'text';
    this.elementRef.nativeElement.style['cursor'] = 'initial';
  }
}
