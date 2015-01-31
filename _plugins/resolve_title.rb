# encoding: utf-8

module Jekyll
  module ResolveTitleFilter
    
    def resolve_title(raw)
      if TITLE_MAP.has_key?(raw)
        raw = TITLE_MAP[raw]
      end
      raw
    end
    
  private

    TITLE_MAP = {
        "dotnet"  => ".NET",
        "latex"  => "LaTeX",
        "dev" => "Разработка",
        "r" => "R",
        "algo" => "Алгоритмы",
        "science" => "Наука",
        "activities" => "Мероприятия"
    }

  end
end

Liquid::Template.register_filter(Jekyll::ResolveTitleFilter)