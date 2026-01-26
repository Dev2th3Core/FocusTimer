'use client'

import { useState, useEffect } from 'react'
import { Button } from '@/components/ui/button'
import { Clock, Github, Download, Zap, Target, Sparkles, Gauge, Timer } from 'lucide-react'
import Image from 'next/image'
import { Badge } from '@/components/ui/badge'

export default function Home() {
  const [isVisible, setIsVisible] = useState<Record<string, boolean>>({})

  useEffect(() => {
    const observers: Record<string, IntersectionObserver> = {}
    const elements = document.querySelectorAll('[data-scroll-animate]')

    elements.forEach((el) => {
      const id = el.getAttribute('data-scroll-animate')
      if (!id) return

      observers[id] = new IntersectionObserver(
        ([entry]) => {
          if (entry.isIntersecting) {
            setIsVisible((prev) => ({ ...prev, [id]: true }))
            observers[id].unobserve(el)
          }
        },
        { threshold: 0.1 }
      )
      observers[id].observe(el)
    })

    return () => {
      Object.values(observers).forEach((observer) => observer.disconnect())
    }
  }, [])

  const roadmapItems = [
    {
      title: 'Session Analytics Dashboard',
      description: 'Track your focus sessions with detailed insights and productivity patterns',
      icon: Gauge,
      color: 'from-blue-500 to-cyan-500',
      progress: 60,
    },
    {
      title: 'Cross-Platform Support',
      description: 'macOS and Linux support for seamless focus tracking everywhere',
      icon: Zap,
      color: 'from-purple-500 to-pink-500',
      progress: 40,
    },
    {
      title: 'Focus Profiles & Presets',
      description: 'Save your favorite timer configurations for different tasks',
      icon: Target,
      color: 'from-green-500 to-emerald-500',
      progress: 75,
    },
    {
      title: 'Advanced Customization',
      description: 'Custom fonts, animations, sound effects, and visual themes',
      icon: Sparkles,
      color: 'from-orange-500 to-red-500',
      progress: 50,
    },
  ]

  const handleDownload = () => {
  if (typeof window !== "undefined" && (window as any).gtag) {
    (window as any).gtag("event", "focus_timer_download", {
      event_category: "engagement",
      event_label: "windows_exe",
    });
  }

  window.location.href = "/FocusTimer.exe";
};


  return (
    <main className="min-h-screen bg-gradient-to-b from-background via-background to-background overflow-hidden">
      {/* Navigation */}
      <nav className="fixed top-0 w-full bg-background/80 backdrop-blur-sm border-b border-border z-50">
        <div className="max-w-6xl mx-auto px-6 py-4 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-lg bg-gradient-to-br from-blue-500 to-blue-600 flex items-center justify-center">
              <Timer className="w-5 h-5 text-white" />
            </div>
            <span className="font-semibold text-lg">Focus Timer</span>
          </div>
          <div className="flex items-center gap-4">
            <Button asChild size="sm" className="bg-blue-600 hover:bg-blue-700 cursor-pointer text-white shadow-lg shadow-blue-600/20 transition-all hover:scale-105" onClick={handleDownload}>
                <span>
                  <Download className="w-4 h-4 mr-2" />
                  Download
                </span>
            </Button>
          </div>
        </div>
      </nav>

      {/* Hero Section */}
      <section className="py-20 px-6">
        <div className="max-w-6xl mx-auto">
          <div className="flex flex-col items-center gap-16">
            <div className="space-y-8 text-center max-w-3xl mx-auto">
              <Badge variant="secondary" className="w-fit mx-auto">
                <Sparkles className="w-3 h-3 mr-1" />
                Productivity Reimagined
              </Badge>

              <div className="space-y-4">
                <h1 className="text-5xl font-bold leading-tight tracking-tight text-foreground">
                  Track Your Focus Without a Second Screen
                </h1>
                <p className="text-lg text-muted-foreground leading-relaxed">
                  Inspired by productivity setups that rely on separate timer devices, Focus Timer brings your focus clock straight to your working screen.                
                </p>
              </div>

              <div className="space-y-4">
                <Button asChild size="lg" className="text-base cursor-pointer px-8 bg-blue-600 hover:bg-blue-700 text-white shadow-xl shadow-blue-600/20 transition-all hover:scale-105" onClick={handleDownload}>
                    <span>
                      <Download className="w-5 h-5 mr-2" />
                      Download for Windows
                    </span>
                </Button>
                <p className="text-sm text-muted-foreground">
                  Windows only • macOS & Linux coming soon
                </p>
              </div>
            </div>

            <div className="relative w-full max-w-4xl">
              {/* Laptop Screen Mockup */}
              <div className="relative mx-auto border-muted-foreground/30 bg-background border-[12px] md:border-[16px] rounded-t-2xl shadow-2xl">
                {/* Camera notch */}
                <div className="absolute w-16 md:w-24 h-3 md:h-4 -top-3 md:-top-4 left-1/2 -translate-x-1/2 bg-muted-foreground/30 rounded-b-lg"></div>
                
                {/* Screen Content */}
                <div className="w-full overflow-hidden bg-background">
                    <Image
                        src="/TimerCenter.png" // NOTE: Add your screenshot to the `public` folder
                        alt="Focus Timer application screenshot"
                        width={1920}
                        height={1080}
                        className="w-full h-auto"
                        quality={100}
                        priority
                        unoptimized
                    />
                </div>
              </div>
              {/* Laptop Base */}
              <div className="relative mx-auto bg-muted/50 h-4 md:h-6 w-[105%] -left-[2.5%] rounded-b-xl shadow-inner"></div>
            </div>
          </div>
        </div>
      </section>
      
      {/* Features Section with Images/Videos */}
      <section className="py-20 px-6 relative">
        <div className="max-w-6xl mx-auto">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-bold mb-4">Why Focus Timer?</h2>
            <p className="text-lg text-muted-foreground">See it in action</p>
          </div>

          <div className="space-y-20">
            {/* Feature 1 */}
            <div
              data-scroll-animate="feature-1"
              className={`grid md:grid-cols-2 gap-10 items-center transition-all duration-1000 ${
                isVisible['feature-1']
                  ? 'opacity-100 translate-x-0'
                  : 'opacity-0 -translate-x-12'
              }`}
            >
              <div>
                <h3 className="text-3xl font-bold mb-4">Custom Colors Match Your Setup</h3>
                <p className="text-lg text-muted-foreground mb-6 leading-relaxed">
                  Choose any color combination for timer text and background. Ensure visibility on any screen, from white to dark backgrounds.
                </p>
                <ul className="space-y-3">
                  {['RGB color picker', 'Preset themes', 'Save favorites'].map((item) => (
                    <li key={item} className="flex items-center gap-3">
                      <div className="w-2 h-2 rounded-full bg-primary" />
                      <span className="text-foreground">{item}</span>
                    </li>
                  ))}
                </ul>
              </div>
              <div className="relative rounded-xl overflow-hidden border border-border/50 h-80 bg-muted/20">
                <Image
                  src="/ColorPicker.png"
                  alt="Color picker preview"
                  fill
                  className="object-contain"
                  quality={100}
                  unoptimized
                />
              </div>
            </div>

            {/* Feature 2 */}
            <div
              data-scroll-animate="feature-2"
              className={`grid md:grid-cols-2 gap-10 items-center transition-all duration-1000 ${
                isVisible['feature-2']
                  ? 'opacity-100 translate-x-0'
                  : 'opacity-0 translate-x-12'
              }`}
            >
              <div className="relative rounded-xl overflow-hidden border border-border/50 h-80 order-2 md:order-1 bg-muted/20">
                <Image
                  src="/AlwaysOnTop.png"
                  alt="Always-on-top timer overlay demo"
                  fill
                  className="object-contain"
                  quality={100}
                  unoptimized
                />
              </div>
              <div className="order-1 md:order-2">
                <h3 className="text-3xl font-bold mb-4">Always On Top Overlay</h3>
                <p className="text-lg text-muted-foreground mb-6 leading-relaxed">
                  Timer floats above every application. Work without looking away from your screen. Maximize focus by keeping your timer always visible.
                </p>
                <ul className="space-y-3">
                  {['Stays visible always', 'Zero distractions', 'Transparent mode available'].map((item) => (
                    <li key={item} className="flex items-center gap-3">
                      <div className="w-2 h-2 rounded-full bg-primary" />
                      <span className="text-foreground">{item}</span>
                    </li>
                  ))}
                </ul>
              </div>
            </div>

            {/* Feature 3 */}
            <div
              data-scroll-animate="feature-3"
              className={`grid md:grid-cols-2 gap-10 items-center transition-all duration-1000 ${
                isVisible['feature-3']
                  ? 'opacity-100 translate-x-0'
                  : 'opacity-0 -translate-x-12'
              }`}
            >
              <div>
                <h3 className="text-3xl font-bold mb-4">Position Anywhere on Screen</h3>
                <p className="text-lg text-muted-foreground mb-6 leading-relaxed">
                  Place your timer in any corner or position. Adapt to your unique workflow and screen layout.
                </p>
                <ul className="space-y-3">
                  {['9-position grid', 'Pixel-perfect placement', 'Drag to reposition'].map((item) => (
                    <li key={item} className="flex items-center gap-3">
                      <div className="w-2 h-2 rounded-full bg-primary" />
                      <span className="text-foreground">{item}</span>
                    </li>
                  ))}
                </ul>
              </div>
              <div className="relative rounded-xl overflow-hidden border border-border/50 h-80 bg-muted/20">
                <Image
                  src="/Multi-Position.png"
                  alt="Multi-position timer placement demo"
                  fill
                  className="object-contain"
                  quality={100}
                  unoptimized
                />
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Exciting Roadmap Section */}
      <section className="py-24 px-6 relative overflow-hidden">
        <div className="absolute inset-0 bg-gradient-to-br from-primary/5 via-transparent to-transparent" />
        <div className="max-w-6xl mx-auto relative z-10">
          <div className="text-center mb-16">
            <h2 className="text-4xl md:text-5xl font-bold mb-4">What's Next</h2>
            <p className="text-lg text-muted-foreground">Exciting features coming to Focus Timer</p>
          </div>

          {/* Grid layout */}
          <div className="grid md:grid-cols-3 gap-6">
            {roadmapItems.map((item, idx) => {
              const IconComponent = item.icon
              return (
                <div
                  key={idx}
                  data-scroll-animate={`roadmap-${idx}`}
                  className={`group transition-all duration-1000 ${
                    isVisible[`roadmap-${idx}`]
                      ? 'opacity-100 translate-y-0'
                      : 'opacity-0 translate-y-12'
                  }`}
                >
                  <div className="relative h-full">
                    <div className="absolute inset-0 bg-gradient-to-br from-primary/10 to-transparent rounded-xl opacity-0 group-hover:opacity-100 transition-opacity duration-300 blur-lg" />
                    
                    <div className="relative bg-card border border-border/40 group-hover:border-primary/40 rounded-xl p-6 transition-all duration-300 group-hover:shadow-lg group-hover:shadow-primary/5 h-full flex flex-col">
                      {/* Header with icon and title */}
                      <div className="flex items-start gap-4 mb-4">
                        <div className={`flex-shrink-0 p-2 rounded-lg bg-gradient-to-br ${item.color}`}>
                          <IconComponent className="w-5 h-5 text-white" />
                        </div>
                        <div className="flex-1 min-w-0">
                          <h3 className="text-lg font-bold group-hover:text-primary transition-colors">
                            {item.title}
                          </h3>
                        </div>
                      </div>

                      {/* Description */}
                      <p className="text-muted-foreground text-sm leading-relaxed mb-4 flex-grow">
                        {item.description}
                      </p>

                      {/* Progress bar */}
                      <div className="space-y-2">
                        <div className="flex items-center justify-between text-xs">
                          <span className="text-muted-foreground">Progress</span>
                          <span className={`font-semibold bg-gradient-to-r ${item.color} bg-clip-text text-transparent`}>
                            {item.progress}%
                          </span>
                        </div>
                        <div className="w-full h-2 bg-border/30 rounded-full overflow-hidden">
                          <div
                            className={`h-full bg-gradient-to-r ${item.color} rounded-full transition-all duration-1000`}
                            style={{
                              width: isVisible[`roadmap-${idx}`] ? `${item.progress}%` : '0%',
                            }}
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              )
            })}
          </div>
        </div>
      </section>

      {/* Open Source Section */}
      <section className="py-20 px-6 relative">
        <div className="max-w-2xl mx-auto text-center">
          <div
            data-scroll-animate="open-source"
            className={`transition-all duration-1000 ${
              isVisible['open-source']
                ? 'opacity-100 scale-100'
                : 'opacity-0 scale-95'
            }`}
          >
            <h2 className="text-4xl font-bold mb-4">Built Open Source</h2>
            <p className="text-lg text-muted-foreground mb-8">
              Focus Timer is openly available on GitHub and welcomes community contributions.
            </p>
            <a href="https://github.com/dev2th3core/FocusTimer" target="_blank" rel="noopener noreferrer">
              <Button size="lg" variant="outline" className="gap-2 border-primary/30 hover:border-primary bg-transparent cursor-pointer">
                <Github className="w-5 h-5" />
                View on GitHub
              </Button>
            </a>
          </div>
        </div>
      </section>

      {/* Download CTA Section */}
      <section className="py-20 px-6 relative overflow-hidden">
        <div className="absolute inset-0 bg-gradient-to-r from-primary/20 via-accent/10 to-primary/20" />
        <div className="max-w-2xl mx-auto text-center relative z-10">
          <div
            data-scroll-animate="final-cta"
            className={`transition-all duration-1000 ${
              isVisible['final-cta']
                ? 'opacity-100 translate-y-0'
                : 'opacity-0 translate-y-8'
            }`}
          >
            <h2 className="text-4xl md:text-5xl font-bold mb-4">Start Your Focus Journey Today</h2>
            <p className="text-xl text-muted-foreground mb-10">Free • No signup • Lightweight • Open source</p>
              <Button size="lg" className="text-base cursor-pointer px-4 bg-blue-600 hover:bg-blue-700 text-white shadow-xl shadow-blue-600/20 transition-all hover:scale-105" onClick={handleDownload}>
                <span className="flex items-center gap-2">
                  <Download className="w-5 h-5" />
                  Download Now
                </span>
              </Button>
            <p className="text-sm text-muted-foreground mt-6">Windows • macOS & Linux coming soon</p>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="border-t border-border/30 bg-background/50 py-12 px-6">
        <div className="max-w-6xl mx-auto">
          <div className="grid md:grid-cols-3 gap-8 mb-8">
            <div>
              <div className="flex items-center gap-2 mb-2">
                <Clock className="w-5 h-5 text-primary" />
                <span className="font-semibold">Focus Timer</span>
              </div>
              <p className="text-sm text-muted-foreground">Never lose focus again.</p>
            </div>
            <div>
              <h4 className="font-semibold text-foreground mb-4">Links</h4>
              <ul className="space-y-2">
                <li>
                  <a
                    href="https://github.com"
                    target="_blank"
                    rel="noopener noreferrer"
                    className="text-sm text-muted-foreground hover:text-primary transition-colors"
                  >
                    GitHub Repository
                  </a>
                </li>
                <li>
                  <a
                    href="mailto:dev2th3core@gmail.com"
                    className="text-sm text-muted-foreground hover:text-primary transition-colors"
                  >
                    Contact Us
                  </a>
                </li>
              </ul>
            </div>
            <div>
              <h4 className="font-semibold text-foreground mb-4">Status</h4>
              <p className="text-sm text-muted-foreground">Windows • macOS & Linux coming soon</p>
            </div>
          </div>
          <div className="border-t border-border/30 pt-8">
            <p className="text-sm text-muted-foreground text-center">© 2026 Focus Timer. All rights reserved.</p>
          </div>
        </div>
      </footer>
    </main>
  )
}
