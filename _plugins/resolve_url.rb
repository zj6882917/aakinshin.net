# encoding: utf-8

module Jekyll
  module ResolveUrlFilter
    
    def resolve_url(raw)
      char_map = { "#"  => "sharp", "."  => "dot", " "  => "-" }
      s = ""
      raw.each_char do |c|
        if char_map.has_key?(c)
          s += char_map[c]
        else
          s += c
        end
      end
      s.downcase
    end

  end
end

Liquid::Template.register_filter(Jekyll::ResolveUrlFilter)